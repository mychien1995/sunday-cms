using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Persistence.Entities;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;
using Sunday.Foundation.Context;
using static LanguageExt.Prelude;

namespace Sunday.ContentManagement.Implementation.Services
{
    [ServiceTypeOf(typeof(IContentService))]
    public class DefaultContentService : IContentService
    {
        private readonly IContentRepository _contentRepository;
        private readonly ISundayContext _sundayContext;
        private readonly IFieldTypesProvider _fieldTypesProvider;
        private readonly ITemplateService _templateService;

        public DefaultContentService(ISundayContext sundayContext, IContentRepository contentRepository,
            IFieldTypesProvider fieldTypesProvider, ITemplateService templateService)
        {
            _sundayContext = sundayContext;
            _contentRepository = contentRepository;
            _fieldTypesProvider = fieldTypesProvider;
            _templateService = templateService;
        }

        public Task<Content[]> GetChildsAsync(Guid contentId, ContentType contentType)
           => _contentRepository.GetByParentAsync(contentId, (int)contentType)
                .MapResultTo(rs => rs.Select(item => item.MapTo<Content>()).ToArray());

        public Task<Content[]> GetMultiples(Guid[] contentIds)
            => _contentRepository.GetMultiples(contentIds)
                .MapResultTo(rs => rs.Select(item => item.MapTo<Content>()).ToArray());

        public async Task<Option<Content>> GetByIdAsync(Guid contentId, GetContentOptions? options = null)
        {
            var contentOpt = await _contentRepository.GetByIdAsync(contentId, options);
            if(contentOpt.IsNone) return Option<Content>.None;
            return await ToModel(contentOpt.Get());
        }

        public async Task CreateAsync(Content content)
        {
            await ApplicationPipelines.RunAsync("cms.entity.beforeCreate", new BeforeCreateEntityArg(content));
            await ApplicationPipelines.RunAsync("cms.content.beforeCreate", new BeforeCreateContentArg(content));
            await _contentRepository.CreateAsync(await ToEntity(content));
        }

        public async Task UpdateAsync(Content content)
        {
            await ApplicationPipelines.RunAsync("cms.entity.beforeUpdate", new BeforeUpdateEntityArg(content));
            await ApplicationPipelines.RunAsync("cms.content.beforeUpdate", new BeforeUpdateContentArg(content));
            await _contentRepository.UpdateAsync(await ToEntity(content));
        }

        public async Task DeleteAsync(Guid contentId)
        {
            await ApplicationPipelines.RunAsync("cms.content.beforeDelete", new BeforeDeleteContentArg(contentId));
            await _contentRepository.DeleteAsync(contentId);
        }

        public Task NewVersionAsync(Guid contentId, Guid fromVersionId)
            => _contentRepository.CreateNewVersionAsync(contentId, fromVersionId, _sundayContext.CurrentUser!.UserName,
                DateTime.Now);

        public Task PublishAsync(Guid contentId)
            => _contentRepository.PublishAsync(contentId, _sundayContext.CurrentUser!.UserName, DateTime.Now);

        private async Task<Content> ToModel(ContentEntity entity)
        {
            var model = entity.MapTo<Content>();
            var fieldTypeDict = (await _templateService.LoadTemplateFields(entity.TemplateId)).ToDictionary(k => k.Id);
            model.Template = entity.Template.MapTo<Template>();
            model.Fields = entity.Fields.Select(ResolveContentField).ToArray();
            model.Versions = entity.Versions.Select(version =>
            {
                var versionModel = version.MapTo<WorkContent>();
                versionModel.Fields = version.Fields.Select(ResolveContentField).ToArray();
                return versionModel;
            }).ToArray();
            return model;

            ContentField ResolveContentField(ContentFieldEntity field)
                => ResolveFieldFromRawValue(field.Id, field.FieldValue, field.TemplateFieldId,
                    fieldTypeDict[field.TemplateFieldId].FieldType);
        }

        ContentField ResolveFieldFromRawValue(Guid id, string? value, Guid templateFieldId, int templateFieldCode)
        {
            var fieldType = _fieldTypesProvider.Get(templateFieldCode);
            var objectValue = fieldType.Handler.Deserialize(value);
            return new ContentField
            {
                TemplateFieldId = templateFieldId,
                TemplateFieldCode = templateFieldCode,
                Id = id,
                FieldValue = objectValue
            }; ;
        }

        private async Task<ContentEntity> ToEntity(Content content)
        {
            var templateFields = await _templateService.LoadTemplateFields(content.TemplateId);
            var fieldTypeDict = templateFields.ToDictionary(k => k.Id);
            var entity = content.MapTo<ContentEntity>();
            entity.Fields = content.Fields.Select(ResolveEntityField).ToArray();
            entity.Versions = content.Versions.Select(version =>
            {
                var versionEntity = version.MapTo<WorkContentEntity>();
                versionEntity.Fields = version.Fields.Select(ResolveEntityField).ToArray();
                return versionEntity;
            }).ToArray();
            return entity;

            ContentFieldEntity ResolveEntityField(ContentField field)
                => ResolveEntityFieldFromValue(field.Id, field.FieldValue, field.TemplateFieldId, fieldTypeDict[field.TemplateFieldId].FieldType);
        }

        ContentFieldEntity ResolveEntityFieldFromValue(Guid id, object? value, Guid templateFieldId, int templateFieldCode)
        {
            var fieldType = _fieldTypesProvider.Get(templateFieldCode);
            var stringValue = fieldType.Handler.Serialize(value);
            return new ContentFieldEntity
            {
                TemplateFieldId = templateFieldId,
                Id = id,
                FieldValue = stringValue
            }; ;
        }
    }
}
