using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Application;
using Sunday.Core.Extensions;

namespace Sunday.ContentDelivery.Implementation.Services
{
    [ServiceTypeOf(typeof(IRenderingReader), LifetimeScope.Singleton)]
    public class DefaultRenderingReader : IRenderingReader
    {
        private readonly IRenderingService _renderingService;
        private static readonly ConcurrentDictionary<Guid, Rendering> RenderingCache = new ConcurrentDictionary<Guid, Rendering>();

        public DefaultRenderingReader(IRenderingService renderingService, IRemoteEventHandler remoteEventHandler)
        {
            _renderingService = renderingService;
            remoteEventHandler.Subscribe(data =>
            {
                if (!data.EventName.Equals("rendering:updated") && !data.EventName.Equals("rendering:deleted")) return;
                var renderingId = Guid.Parse(data.Data!.ToString()!);
                RenderingCache.TryRemove(renderingId, out _);
            });
        }

        public async Task<Option<Rendering>> GetRendering(Guid renderingId)
        {
            if (RenderingCache.ContainsKey(renderingId)) return RenderingCache[renderingId];
            var renderingOpt = await _renderingService.GetRenderingById(renderingId);
            if (renderingOpt.IsNone) return Option<Rendering>.None;
            var rendering = renderingOpt.Get();
            RenderingCache[renderingId] = rendering;
            return rendering;
        }
    }
}
