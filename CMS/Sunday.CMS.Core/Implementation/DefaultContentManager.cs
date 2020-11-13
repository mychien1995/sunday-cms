using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Contents;
using Sunday.ContentManagement;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Implementation
{
    [ServiceTypeOf(typeof(IContentManager))]
    public class DefaultContentManager : IContentManager
    {
        private readonly IContentService _contentService;

        public DefaultContentManager(IContentService contentService)
        {
            _contentService = contentService;
        }

        public async Task<ContentJsonResult> GetContentByIdAsync(Guid contentId, Guid? versionId = null)
        {
            var contentOpt = await _contentService.GetByIdAsync(contentId,
                new GetContentOptions { IncludeFields = true, IncludeVersions = true });
            if (contentOpt.IsNone) return BaseApiResponse.ErrorResult<ContentJsonResult>("Content not found");
            var content = contentOpt.Get();
            var version = content.Versions.FirstOrDefault(v => v.Id == versionId) ??
                          content.Versions.FirstOrDefault(v => v.IsActive);
            if (version == null)
                return BaseApiResponse.ErrorResult<ContentJsonResult>("Content not found");
            var jsonResult = content.MapTo<ContentJsonResult>();
            jsonResult.Versions =
                content.Versions.Select(v => new ContentVersion { Version = v.Version, VersionId = v.Id, IsActive = v.IsActive, Status = v.Status }).ToArray();
            jsonResult.Fields = version.Fields.Select(f => new ContentFieldItem
            { FieldValue = f.FieldValue, Id = f.Id, TemplateFieldId = f.TemplateFieldId }).ToArray();
            jsonResult.Template = new ContentTemplate { Icon = content.Template.Icon, TemplateName = content.Template.TemplateName };
            return jsonResult;
        }

        public async Task<CreateContentJsonResult> CreateContentAsync(ContentJsonResult content)
        {
            var model = content.MapTo<Content>();
            var version = new WorkContent
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                Fields = content.Fields.Select(f => new WorkContentField
                {
                    Id = Guid.NewGuid(),
                    FieldValue = f.FieldValue,
                    TemplateFieldId = f.TemplateFieldId
                }).ToArray(),
                Status = (int)ContentStatuses.Draft
            };
            model.Versions = new[] { version };
            await _contentService.CreateAsync(model);
            return new CreateContentJsonResult(model.Id);
        }

        public async Task<BaseApiResponse> UpdateContentAsync(ContentJsonResult content)
        {
            var model = content.MapTo<Content>();
            var activeVersion = content.Versions.FirstOrDefault(v => v.IsActive)!;
            var version = new WorkContent
            {
                Id = activeVersion.VersionId,
                IsActive = true,
                Fields = content.Fields.Select(f => new WorkContentField
                {
                    Id = f.Id == Guid.Empty ? Guid.NewGuid() : f.Id,
                    FieldValue = f.FieldValue,
                    TemplateFieldId = f.TemplateFieldId
                }).ToArray()
            };
            model.Versions = new[] { version };
            await _contentService.UpdateAsync(model);
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> DeleteContentAsync(Guid contentId)
        {
            await _contentService.DeleteAsync(contentId);
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> NewContentVersionAsync(Guid contentId, Guid fromVersion)
        {
            await _contentService.NewVersionAsync(contentId, fromVersion);
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> PublishContentAsync(Guid contentId)
        {
            await _contentService.PublishAsync(contentId);
            return BaseApiResponse.SuccessResult;
        }
    }
}
