using System;

namespace Sunday.Core.Application
{
    public interface IRemoteEventHandler
    {
        void Initialize();
        void Send(RemoteEventData message);

        void OnReceive(RemoteEventData action);
        void Subscribe(Action<RemoteEventData> action);
    }

    public class RemoteEventData
    {
        public RemoteEventData(string eventName, object data)
        {
            EventName = eventName;
            Data = data;
        }

        public string EventName { get; }
        public object Data { get; }
    }

    public class RemoveEventReceivedArg : EventArgs
    {
        public RemoteEventData Data;

        public RemoveEventReceivedArg(RemoteEventData data)
        {
            Data = data;
        }
    }

    public delegate void OnReceiveMessageHandler(object sender, RemoveEventReceivedArg arg);
}
