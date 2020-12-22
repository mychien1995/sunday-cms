using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Application;
using Sunday.Core.Extensions;

namespace Sunday.ContentDelivery.Implementation.Services
{
    [ServiceTypeOf(typeof(ILayoutReader), LifetimeScope.Singleton)]
    public class DefaultLayoutReader : ILayoutReader
    {
        private readonly ILayoutService _layoutService;
        private static readonly ConcurrentDictionary<Guid, string> LayoutCache = new ConcurrentDictionary<Guid, string>();

        public DefaultLayoutReader(ILayoutService layoutService, IRemoteEventHandler remoteEventHandler)
        {
            _layoutService = layoutService; 
            remoteEventHandler.Subscribe(data =>
            {
                if (!data.EventName.Equals("layout:updated") && !data.EventName.Equals("layout:deleted")) return;
                var layoutId = Guid.Parse(data.Data!.ToString()!);
                LayoutCache.TryRemove(layoutId, out _);
            });
        }

        public async Task<string?> GetLayout(Guid layoutId)
        {
            if (LayoutCache.ContainsKey(layoutId)) return LayoutCache[layoutId];
            var layoutOpt = await _layoutService.GetByIdAsync(layoutId);
            if (layoutOpt.IsNone) return null;
            var layoutPath = layoutOpt.Get().LayoutPath;
            LayoutCache[layoutId] = layoutOpt.Get().LayoutPath;
            return layoutPath;
        }
    }
}
