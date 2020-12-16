using System;
using Sunday.ContentManagement.Services;
using Sunday.Core.Application;
using Sunday.Core.Extensions;
using Sunday.Core.Implementation;
using Sunday.Core.Pipelines;

namespace Sunday.ContentDelivery.Framework.Pipelines
{
    public class HandleRemoteEvent : IPipelineProcessor
    {
        private readonly IRemoteEventHandler _remoteEventHandler;
        private readonly IEntityCacheManager _entityCacheManager;
        private readonly IContentService _contentService;

        public HandleRemoteEvent(IRemoteEventHandler remoteEventHandler, IEntityCacheManager entityCacheManager, IContentService contentService)
        {
            _remoteEventHandler = remoteEventHandler;
            _entityCacheManager = entityCacheManager;
            _contentService = contentService;
        }

        public void Process(PipelineArg arg)
        {
            if (_remoteEventHandler is TcpRemoteEventHandler tcpRemoteEventHandler)
            {
                tcpRemoteEventHandler.Subscribe(data =>
                {
                    if (data.EventName.Equals("content:published"))
                    {
                        var guidId = (Guid)data.Data;
                        var contentOpt = _contentService.GetByIdAsync(guidId).Result;
                        _entityCacheManager.Remove(contentOpt.Get());
                    }
                });
            }
        }
    }
}
