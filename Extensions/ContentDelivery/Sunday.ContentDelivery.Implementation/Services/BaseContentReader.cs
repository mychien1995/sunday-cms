using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement;
using Sunday.ContentManagement.Extensions;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core.Extensions;
using static LanguageExt.Prelude;

namespace Sunday.ContentDelivery.Implementation.Services
{
    public abstract class BaseContentReader : IContentReader
    {
        protected readonly IContentPathResolver ContentPathResolver;
        protected readonly IContentService ContentService;
        protected readonly ITemplateService TemplateService;

        protected BaseContentReader(IContentPathResolver contentPathResolver, IContentService contentService, ITemplateService templateService)
        {
            ContentPathResolver = contentPathResolver;
            ContentService = contentService;
            TemplateService = templateService;
        }

        public virtual async Task<Option<Content>> GetHomePage(Guid websiteId)
        {
            var siteRoots = await ContentService.GetChildsAsync(websiteId, ContentType.Website);
            var templates = await TemplateService.QueryAsync(new TemplateQuery
            {
                PageIndex = 0,
                PageSize = siteRoots.Length,
                IncludeIds = siteRoots.Select(c => c.TemplateId.ToString()).ToArray()
            });
            return Optional(siteRoots.FirstOrDefault(content => templates.Result.Any(t =>
                                                                          t.Id == content.TemplateId &&
                                                                          t.IsPageTemplate)));
        }

        public async Task<Option<Content>> GetPage(Guid websiteId, string path)
        {
            var homePage = await GetHomePage(websiteId);
            if (homePage.IsNone) return Option<Content>.None;
            var formalizedPath = path.FormalizeAsContentPath();
            if (string.IsNullOrEmpty(formalizedPath)) return homePage;
            var fullPath = $"{homePage.Get().Name}/{formalizedPath}";
            var contentOpt = await ContentPathResolver.GetContentByNamePath(websiteId, fullPath);
            if (contentOpt.Some(c => c.IsPublished).None(true)) return Option<Content>.None;
            var content = await GetContent(contentOpt.Get().Id);
            return content;
        }

        public virtual Task<Option<Content>> GetContent(Guid contentId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<Content[]> GetChilds(Guid parentId, ContentType contentType)
            => ContentService.GetChildsAsync(parentId, contentType);
    }
}
