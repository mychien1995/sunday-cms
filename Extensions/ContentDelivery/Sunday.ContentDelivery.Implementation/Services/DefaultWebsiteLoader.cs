using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Application;
using Sunday.Core.Extensions;

namespace Sunday.ContentDelivery.Implementation.Services
{
    [ServiceTypeOf(typeof(IWebsiteLoader), LifetimeScope.Singleton)]
    public class DefaultWebsiteLoader : IWebsiteLoader
    {
        private readonly IWebsiteService _websiteService;
        private static ConcurrentDictionary<string, ApplicationWebsite> _websiteCache
            = new ConcurrentDictionary<string, ApplicationWebsite>();

        public DefaultWebsiteLoader(IWebsiteService websiteService, IRemoteEventHandler remoteEventHandler)
        {
            _websiteService = websiteService;
            remoteEventHandler.Subscribe(ev =>
            {
                if (!ev.EventName.Equals("website:changed") && !ev.EventName.Equals("website:deleted")) return;
                _websiteCache = new ConcurrentDictionary<string, ApplicationWebsite>();
            });
        }

        public async Task<Option<ApplicationWebsite>> GetWebsiteByHostName(string hostname)
        {
            if (_websiteCache.ContainsKey(hostname)) return _websiteCache[hostname];
            var websiteOpt = await _websiteService.GetByHostNameAsync(hostname);
            if (websiteOpt.IsNone) return Option<ApplicationWebsite>.None;
            var website = websiteOpt.Get();
            _websiteCache[hostname] = website;
            return website;
        }
    }
}
