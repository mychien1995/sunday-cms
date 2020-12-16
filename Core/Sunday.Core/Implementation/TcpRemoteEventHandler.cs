using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sunday.Core.Application;

namespace Sunday.Core.Implementation
{
    public class TcpRemoteEventHandler : IRemoteEventHandler
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public const string Delimeter = "[pkg_end]";
        private readonly TcpRemoteEventConfiguration _configuration;
        private readonly Dictionary<TcpRemoteAddress, TcpClient> _openingClients;
        private readonly Dictionary<TcpRemoteAddress, int> _renewAttempts;
        private readonly List<TcpRemoteAddress> _addresses;
        private readonly ILogger<TcpRemoteEventHandler> _logger;

        public TcpRemoteEventHandler(ILogger<TcpRemoteEventHandler> logger, TcpRemoteEventConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _openingClients = new Dictionary<TcpRemoteAddress, TcpClient>();
            _renewAttempts = new Dictionary<TcpRemoteAddress, int>();
            _addresses = new List<TcpRemoteAddress>();
        }

        public void Initialize()
        {
            foreach (var address in _configuration.PublishingAddresses)
            {
                _renewAttempts.Add(address, 0);
                _addresses.Add(address);
            }
        }

        public TcpClient? RenewClient(TcpRemoteAddress address)
        {
            try
            {
                var client = new TcpClient(address.Host, address.Port);
                return client;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on opening tcp client");
            }

            return null;
        }

        public void Send(RemoteEventData message)
        {
            Task.Run(() =>
            {
                foreach (var address in _addresses)
                {
                    TcpClient client;
                    if (_openingClients.ContainsKey(address) && _openingClients[address].Connected)
                    {
                        client = _openingClients[address];
                    }
                    else
                    {
                        client = RenewClient(address)!;
                    }

                    if (client == null)
                    {
                        _renewAttempts[address] = _renewAttempts[address] + 1;
                        if (_renewAttempts[address] > 5)
                        {
                            _addresses.Remove(address);
                            _logger.LogError($"Maximum retry passed for {address}, drop this address");
                        }

                        return;
                    }
                    _openingClients[address] = client;
                    var stream = client.GetStream();
                    var dataMessage = JsonConvert.SerializeObject(message);
                    var data = System.Text.Encoding.ASCII.GetBytes(dataMessage + Delimeter);
                    stream.Write(data, 0, data.Length);
                }
            });
        }

        public void OnReceive(RemoteEventData action)
        {
            OnReceiveEvent(this, new RemoveEventReceivedArg(action));
        }

        public void Subscribe(Action<RemoteEventData> action)
        {
            this.OnReceiveEvent += (sender, arg) => { action(arg.Data); };
        }

        event OnReceiveMessageHandler OnReceiveEvent;
    }
}
