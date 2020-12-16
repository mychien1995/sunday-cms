namespace Sunday.Core.Domain
{
    public class RemoteEventConfiguration
    {
        public RemoteEventConfiguration(string remoteEventProvider)
        {
            RemoteEventProvider = remoteEventProvider;
        }

        public string RemoteEventProvider { get; }
    }
}
