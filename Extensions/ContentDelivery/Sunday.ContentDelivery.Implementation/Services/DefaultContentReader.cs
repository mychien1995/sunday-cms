using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.UnitsOfMeasure;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement;
using Sunday.ContentManagement.Extensions;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Application;
using Sunday.Core.Extensions;
using static LanguageExt.Prelude;

namespace Sunday.ContentDelivery.Implementation.Services
{
    [ServiceTypeOf(typeof(IContentReader))]
    public class DefaultContentReader : IContentReader
    {
        private readonly IContentPathResolver _contentPathResolver;
        private readonly IContentService _contentService;
        private readonly ITemplateService _templateService;
        private readonly IEntityCacheManager _cacheManager;
        private readonly ConcurrentDictionary<Guid, Guid> _siteHomePageMappings = new ConcurrentDictionary<Guid, Guid>();

        public DefaultContentReader(IContentPathResolver contentPathResolver, IContentService contentService,
            ITemplateService templateService, IEntityCacheManager cacheManager, IRemoteEventHandler remoteEventHandler)
        {
            _contentPathResolver = contentPathResolver;
            _contentService = contentService;
            _templateService = templateService;
            _cacheManager = cacheManager;
            remoteEventHandler.Subscribe(data =>
            {
                if (!data.EventName.Equals("content:moved") && !data.EventName.Equals("content:deleted")) return;
                var contentId = Guid.Parse(data.Data.ToString()!);
                if (_siteHomePageMappings.All(kv => kv.Value != contentId)) return;
                var site = _siteHomePageMappings.FirstOrDefault(v => v.Value == contentId).Key;
                _siteHomePageMappings.TryRemove(site, out _);
            });
        }

        public async Task<Option<Content>> GetHomePage(Guid websiteId)
        {
            if (_siteHomePageMappings.TryGetValue(websiteId, out var homePageId))
            {
                return await LoadContentFromCache(homePageId);
            }
            var siteRoots = await _contentService.GetChildsAsync(websiteId, ContentType.Website);
            var templates = await _templateService.QueryAsync(new TemplateQuery
            {
                PageIndex = 0,
                PageSize = siteRoots.Length,
                IncludeIds = siteRoots.Select(c => c.TemplateId.ToString()).ToArray()
            });
            return await Optional(siteRoots.FirstOrDefault(content => content.IsPublished &&
                                                                      templates.Result.Any(t =>
                                                                          t.Id == content.TemplateId &&
                                                                          t.IsPageTemplate)))
                .MatchAsync(async page =>
                {
                    _siteHomePageMappings[websiteId] = page!.Id;
                    return await LoadContentFromCache(page!.Id);
                }, () => Option<Content>.None);
        }

        private Task<Option<Content>> LoadContentFromCache(Guid contentId)
        => _cacheManager.ReadThrough(contentId, () => LoadContent(contentId), 1.Hours());
        private async Task<Option<Content>> LoadContent(Guid contentId)
        {
            var contentOpt = await _contentService.GetByIdAsync(contentId,
                new GetContentOptions { IncludeFields = true });
            if (contentOpt.IsNone) return Option<Content>.None;
            var content = contentOpt.Get();
            if (!content.IsPublished) return Option<Content>.None;
            var fields = await _templateService.LoadTemplateFields(content.TemplateId);
            content.Template.Fields = fields;
            return Optional(content);
        }
        public async Task<Option<Content>> GetPage(Guid websiteId, string path)
        {
            var homePage = await GetHomePage(websiteId);
            if (homePage.IsNone) return Option<Content>.None;
            var formalizedPath = path.FormalizeAsContentPath();
            if (string.IsNullOrEmpty(formalizedPath)) return homePage;
            var fullPath = $"{homePage.Get().Name}/{formalizedPath}";
            var contentOpt = await _contentPathResolver.GetContentByNamePath(websiteId, fullPath);
            if (contentOpt.Some(c => c.IsPublished).None(true)) return Option<Content>.None;
            var content = await GetContent(contentOpt.Get().Id);
            return content;
        }

        public Task<Option<Content>> GetContent(Guid contentId)
            => LoadContentFromCache(contentId);
    }
}
