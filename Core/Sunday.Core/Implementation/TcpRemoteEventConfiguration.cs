using System;
using System.Linq;

namespace Sunday.Core.Implementation
{
    public class TcpRemoteEventConfiguration
    {
        public TcpRemoteEventConfiguration(string listeningAddress, string publishingAddress)
        {
            if (!string.IsNullOrEmpty(listeningAddress))
                ListeningAddress = int.Parse(listeningAddress);
            if (!string.IsNullOrEmpty(publishingAddress))
                PublishingAddresses = publishingAddress.Split('|').Select(p => new TcpRemoteAddress(p)).ToArray();
            else PublishingAddresses = Array.Empty<TcpRemoteAddress>();
        }

        public int ListeningAddress { get; }
        public TcpRemoteAddress[] PublishingAddresses { get; }
    }

    public class TcpRemoteAddress
    {
        public TcpRemoteAddress(string address)
        {
            var part = address.Split(':');
            Host = part[0];
            Port = int.Parse(part[1]);
        }
        public TcpRemoteAddress(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public string Host { get; }
        public int Port { get; }

        public override string ToString() => $"{Host}:{Port}";
    }
}
