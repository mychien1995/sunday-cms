using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core.Extensions;
using static LanguageExt.Prelude;

namespace Sunday.ContentDelivery.Implementation.Services
{
    public class UnpublishedContentReader : BaseContentReader
    {

        public UnpublishedContentReader(ITemplateService templateService, IContentService contentService, IContentPathResolver contentPathResolver) : base(contentPathResolver
        , contentService, templateService)
        {
        }

        public override async Task<Option<Content>> GetHomePage(Guid websiteId)
        {
            return await base.GetHomePage(websiteId)
                .MatchAsync(async page => await LoadContent(page!.Id), () => Option<Content>.None);
        }
        private async Task<Option<Content>> LoadContent(Guid contentId)
        {
            var contentOpt = await ContentService.GetByIdAsync(contentId,
                new GetContentOptions { IncludeFields = true, IncludeVersions = true});
            if (contentOpt.IsNone) return Option<Content>.None;
            var content = contentOpt.Get();
            var fields = await TemplateService.LoadTemplateFields(content.TemplateId);
            content.Template.Fields = fields;
            return Optional(content);
        }


        public override Task<Option<Content>> GetContent(Guid contentId)
            => LoadContent(contentId);

        public override async Task<Content[]> GetChilds(Guid parentId, ContentType contentType)
        {
            var contents = await ContentService.GetChildsAsync(parentId, contentType);
            var fullContents = await Task.WhenAll(contents.Select(c => GetContent(c.Id)))
                .MapResultTo(rs => rs.Where(c => c.IsSome).Select(c => c.Get()).ToArray());
            return fullContents;
        }
    }
}
