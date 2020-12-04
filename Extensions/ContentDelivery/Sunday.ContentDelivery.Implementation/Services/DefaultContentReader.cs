using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Extensions;

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


        public async Task<Option<Content>> GetContentByNamePath(Guid websiteId, string path)
        {
            var contentOpt = await _contentPathResolver.GetContentByNamePath(websiteId, path);
            if (contentOpt.IsNone) return Option<Content>.None;
            var content = await _contentService.GetByIdAsync(contentOpt.Get().Id, new GetContentOptions { IncludeFields = true }).MapResultTo(rs => rs.Get());
            var template = await _templateService.GetByIdAsync(content.TemplateId);
            if (template.IsNone) return Option<Content>.None;
            content.Template = template.Get();
            var fields = await _templateService.LoadTemplateFields(content.TemplateId);
            content.Template.Fields = fields;
            return content;
        }

        public Task<Option<Content>> GetContent(Guid contentId)
            => _contentService.GetByIdAsync(contentId, new GetContentOptions { IncludeFields = true});
    }
}
