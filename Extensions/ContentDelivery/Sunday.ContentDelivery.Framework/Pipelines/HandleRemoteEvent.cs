using System;
using Sunday.ContentManagement.Models;
using Sunday.Core.Application;
using Sunday.Core.Pipelines;

namespace Sunday.ContentDelivery.Framework.Pipelines
{
    public class HandleRemoteEvent : IPipelineProcessor
    {
        private readonly IRemoteEventHandler _remoteEventHandler;
        private readonly IEntityCacheManager _entityCacheManager;

        public HandleRemoteEvent(IRemoteEventHandler remoteEventHandler, IEntityCacheManager entityCacheManager)
        {
            _remoteEventHandler = remoteEventHandler;
            _entityCacheManager = entityCacheManager;
        }

        public void Process(PipelineArg arg)
        {
            _remoteEventHandler.Subscribe(data =>
            {
                if (!data.EventName.Equals("content:published") && !data.EventName.Equals("content:deleted")) return;
                var guidId = Guid.Parse(data.Data!.ToString()!);
                _entityCacheManager.Remove(typeof(Content), guidId);
            });
        }
    }
}
