using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sunday.Core.Application;
using Sunday.Core.Implementation;

namespace Sunday.Core.Framework
{
    public class TcpEventListener : BackgroundService
    {
        private readonly ILogger<TcpEventListener> _logger;
        private readonly IRemoteEventHandler _remoteEventHandler;
        private readonly TcpRemoteEventConfiguration _configuration;
        private readonly string[] _allowedIps;
        private readonly int MaximumConnections = 3;
        private int _numberOfConnections = 0;
        private TcpListener? _listener;

        public TcpEventListener(ILogger<TcpEventListener> logger, IRemoteEventHandler remoteEventHandler, IHostApplicationLifetime hostApplicationLifetime,
            TcpRemoteEventConfiguration configuration)
        {
            _logger = logger;
            _remoteEventHandler = remoteEventHandler;
            _configuration = configuration;
            _allowedIps = new[] { "127.0.0.1", "0.0.0.0" }.Concat(configuration.AllowedIps).ToArray();
            hostApplicationLifetime.ApplicationStopping.Register(() =>
            {
                StopAsync(new CancellationToken()).Wait();
            });
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            _listener?.Stop();
            _logger.LogInformation("TCP Listener stopped");
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var delimeter = TcpRemoteEventHandler.Delimeter;
            var address = _configuration.ListeningAddress;
            _listener = new TcpListener(IPAddress.Any, address);
            _listener.Start();
            _logger.LogInformation($"Start listening at TCP port {address}");
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_numberOfConnections >= MaximumConnections) continue;
                var client = await _listener.AcceptTcpClientAsync();
                var clientIp = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                if (!_allowedIps.Contains(clientIp))
                {
                    _logger.LogWarning($"Ignore TCP connection from {clientIp}");
                    client.Close();
                    continue;
                }
                _logger.LogInformation($"A new client connected {clientIp}");
                Interlocked.Increment(ref _numberOfConnections);
                _ = Task.Run(() =>
                {
                    try
                    {
                        StartListening(client);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("TCP Client close because of", ex);
                        Interlocked.Decrement(ref _numberOfConnections);
                        client.Close();
                    }
                }, cancellationToken);
            }

            void StartListening(TcpClient client)
            {
                var stream = client.GetStream();
                var bufferCount = 0;
                var offset = 0;
                var fullBuffer = new byte[512000];
                do
                {
                    byte[] buffer = new byte[1024];
                    int dataRead = stream.ReadAsync(buffer, offset, buffer.Length, cancellationToken).Result;
                    Buffer.BlockCopy(buffer, 0, fullBuffer, bufferCount, dataRead);
                    bufferCount += dataRead;
                    var message = Encoding.ASCII.GetString(fullBuffer, 0, bufferCount);
                    var delimeterIndex = message.IndexOf(delimeter, StringComparison.OrdinalIgnoreCase);
                    if (delimeterIndex > -1)
                    {
                        var dataPart = message.Substring(0, delimeterIndex);
                        var data = JsonConvert.DeserializeObject<RemoteEventData>(dataPart);
                        _logger.LogInformation($"Received event for {data.EventName}");
                        _ = Task.Run(() => _remoteEventHandler.OnReceive(data), cancellationToken);
                        offset = 0;
                        bufferCount = message.Length - delimeterIndex - delimeter.Length;
                        var leftOver = fullBuffer.Skip(delimeterIndex + delimeter.Length).Take(bufferCount).ToArray();
                        fullBuffer = new byte[512000];
                        Buffer.BlockCopy(leftOver, 0, fullBuffer, 0, bufferCount);
                    }
                } while (!cancellationToken.IsCancellationRequested);
            }

        }
    }
}
