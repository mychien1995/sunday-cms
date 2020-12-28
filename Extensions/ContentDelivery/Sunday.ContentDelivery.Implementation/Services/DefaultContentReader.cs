using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.UnitsOfMeasure;
using Sunday.ContentManagement;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core.Application;
using Sunday.Core.Extensions;
using static LanguageExt.Prelude;

namespace Sunday.ContentDelivery.Implementation.Services
{
    public class DefaultContentReader : BaseContentReader
    {
        private readonly IEntityCacheManager _cacheManager;
        private readonly ConcurrentDictionary<Guid, Guid> _siteHomePageMappings = new ConcurrentDictionary<Guid, Guid>();
        private readonly ConcurrentDictionary<Guid, Guid[]> _childCache = new ConcurrentDictionary<Guid, Guid[]>();

        public DefaultContentReader(IContentPathResolver contentPathResolver, IContentService contentService,
            ITemplateService templateService, IEntityCacheManager cacheManager, IRemoteEventHandler remoteEventHandler) : base(contentPathResolver, contentService, templateService)
        {
            _cacheManager = cacheManager;
            remoteEventHandler.Subscribe(data =>
            {
                if (!data.EventName.Equals("content:moved") && !data.EventName.Equals("content:deleted")) return;
                var contentId = Guid.Parse(data.Data.ToString()!);
                new Dictionary<Guid, Guid>(_siteHomePageMappings).Where(kv => kv.Value == contentId)
                    .Iter(kv => _siteHomePageMappings.TryRemove(kv.Key, out _));
                _childCache.TryRemove(contentId, out _);
                new Dictionary<Guid, Guid[]>(_childCache).Where(kv => kv.Value.Contains(contentId))
                    .Iter(kv => _childCache.TryRemove(kv.Key, out _));
                var content = ContentService.GetByIdAsync(contentId).Result;
                content.IfSome(c => _childCache.TryRemove(c.ParentId, out _));
            });
        }

        public override async Task<Option<Content>> GetHomePage(Guid websiteId)
        {
            if (_siteHomePageMappings.TryGetValue(websiteId, out var homePageId))
            {
                return await LoadContentFromCache(homePageId);
            }
            return await base.GetHomePage(websiteId).Where(p => p.IsPublished)
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
            var contentOpt = await ContentService.GetByIdAsync(contentId,
                new GetContentOptions { IncludeFields = true });
            if (contentOpt.IsNone) return Option<Content>.None;
            var content = contentOpt.Get();
            if (!content.IsPublished) return Option<Content>.None;
            var fields = await TemplateService.LoadTemplateFields(content.TemplateId);
            content.Template.Fields = fields;
            return Optional(content);
        }

        public override Task<Option<Content>> GetContent(Guid contentId)
            => LoadContentFromCache(contentId);

        public override async Task<Content[]> GetChilds(Guid parentId, ContentType contentType)
        {
            if (_childCache.ContainsKey(parentId))
            {
                var childIds = _childCache[parentId];
                return await Task.WhenAll(childIds.Select(GetContent))
                    .MapResultTo(rs => rs.Where(c => c.IsSome).Select(c => c.Get()).ToArray());
            }
            var contents = await ContentService.GetChildsAsync(parentId, contentType);
            var fullContents = await Task.WhenAll(contents.Select(c => GetContent(c.Id)))
                .MapResultTo(rs => rs.Where(c => c.IsSome).Select(c => c.Get()).ToArray());
            _childCache[parentId] = contents.Select(c => c.Id).ToArray();
            return fullContents;
        }
    }
}
