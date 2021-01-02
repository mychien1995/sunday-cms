using System;
using System.Linq;

namespace Sunday.Core.Implementation
{
    public class TcpRemoteEventConfiguration
    {
        public TcpRemoteEventConfiguration(string listeningAddress, string publishingAddress, string allowedIps)
        {
            AllowedIps = allowedIps.Split('|');
            if (!string.IsNullOrEmpty(listeningAddress))
                ListeningAddress = int.Parse(listeningAddress);
            PublishingAddresses = !string.IsNullOrEmpty(publishingAddress)
                ? publishingAddress.Split('|').Select(p => new TcpRemoteAddress(p)).ToArray()
                : Array.Empty<TcpRemoteAddress>();
        }

        public int ListeningAddress { get; }
        public TcpRemoteAddress[] PublishingAddresses { get; }
        public string[] AllowedIps { get; }
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
