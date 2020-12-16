using System;
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

namespace Sunday.Core.Framework.Pipelines.Initialize
{
    public class TcpHandlerService : BackgroundService
    {
        private readonly ILogger<TcpHandlerService> _logger;
        private readonly IRemoteEventHandler _remoteEventHandler;
        private readonly TcpRemoteEventConfiguration _configuration;

        public TcpHandlerService(ILogger<TcpHandlerService> logger, IRemoteEventHandler remoteEventHandler, TcpRemoteEventConfiguration configuration)
        {
            _logger = logger;
            _remoteEventHandler = remoteEventHandler;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _remoteEventHandler.Initialize();
            var delimeter = TcpRemoteEventHandler.Delimeter;
            var address = _configuration.ListeningAddress;
            TcpListener listener = new TcpListener(IPAddress.Any, address);
            listener.Start();
            _logger.LogInformation($"Start listening at TCP port {address}");
            TcpClient client = await listener.AcceptTcpClientAsync();
            Console.WriteLine("a new client connected");
            NetworkStream stream = client.GetStream();
            var offset = 0;
            var fullMessage = new StringBuilder();
            do
            {
                byte[] buffer = new byte[1024];
                int dataRead = await stream.ReadAsync(buffer, offset, buffer.Length, cancellationToken);
                var message = Encoding.ASCII.GetString(buffer, 0, dataRead);
                var dIndex = message.IndexOf(delimeter);
                if (dIndex > -1)
                {
                    var beforeDelimeter = message.Substring(0, dIndex);
                    fullMessage.Append(beforeDelimeter);
                    var data = JsonConvert.DeserializeObject<RemoteEventData>(fullMessage.ToString());
                    _remoteEventHandler.OnReceive(data);
                    fullMessage = new StringBuilder();
                    offset = Encoding.ASCII.GetByteCount(beforeDelimeter + delimeter) - dataRead;
                }
                else
                {
                    fullMessage.Append(message);
                    offset = 0;
                }
            } while (!cancellationToken.IsCancellationRequested);
        }
    }
}
