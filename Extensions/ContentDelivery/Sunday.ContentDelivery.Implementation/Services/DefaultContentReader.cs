using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement;
using Sunday.ContentManagement.Extensions;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core;
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

        public DefaultContentReader(IContentPathResolver contentPathResolver, IContentService contentService, ITemplateService templateService)
        {
            _contentPathResolver = contentPathResolver;
            _contentService = contentService;
            _templateService = templateService;
        }

        public async Task<Option<Content>> GetHomePage(Guid websiteId)
        {
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
                                                                          t.IsPageTemplate))).MatchAsync(async page =>
            {
                var fullPage =  await _contentService.GetByIdAsync(page!.Id,
                    new GetContentOptions {IncludeFields = true}).MapResultTo(rs => rs.Get()); 
                var fields = await _templateService.LoadTemplateFields(fullPage.TemplateId);
                fullPage.Template.Fields = fields;
                return Optional(fullPage);
            }, () => Option<Content>.None);
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
            var content = await _contentService.GetByIdAsync(contentOpt.Get().Id, new GetContentOptions { IncludeFields = true }).MapResultTo(rs => rs.Get());
            if (!content.Template.IsPageTemplate) return Option<Content>.None;
            var fields = await _templateService.LoadTemplateFields(content.TemplateId);
            content.Template.Fields = fields;
            return content;
        }

        public Task<Option<Content>> GetContent(Guid contentId)
            => _contentService.GetByIdAsync(contentId, new GetContentOptions { IncludeFields = true })
                .MapResultTo(rs => rs.Bind(c => c.IsPublished ? c : Option<Content>.None));
    }
}
