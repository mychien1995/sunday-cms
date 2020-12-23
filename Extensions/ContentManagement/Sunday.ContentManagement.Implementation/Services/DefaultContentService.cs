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
using Sunday.Core.Application;
using Sunday.Core.Extensions;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;
using Sunday.Foundation.Context;

namespace Sunday.ContentManagement.Implementation.Services
{
    [ServiceTypeOf(typeof(IContentService))]
    public class DefaultContentService : IContentService
    {
        private readonly IContentRepository _contentRepository;
        private readonly IContentOrderRepository _contentOrderRepository;
        private readonly ISundayContext _sundayContext;
        private readonly IFieldTypesProvider _fieldTypesProvider;
        private readonly ITemplateService _templateService;
        private readonly IContentLinkService _contentLinkService;
        private readonly IRemoteEventHandler _remoteEventHandler;

        public DefaultContentService(ISundayContext sundayContext, IContentRepository contentRepository,
            IFieldTypesProvider fieldTypesProvider, ITemplateService templateService, IContentOrderRepository contentOrderRepository, IContentLinkService contentLinkService, IRemoteEventHandler remoteEventHandler)
        {
            _sundayContext = sundayContext;
            _contentRepository = contentRepository;
            _fieldTypesProvider = fieldTypesProvider;
            _templateService = templateService;
            _contentOrderRepository = contentOrderRepository;
            _contentLinkService = contentLinkService;
            _remoteEventHandler = remoteEventHandler;
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
            if (contentOpt.IsNone) return Option<Content>.None;
            return await ToModel(contentOpt.Get());
        }

        public async Task<Option<Content>> GetFullContent(Guid contentId)
        {
            var contentOpt = await GetByIdAsync(contentId,
                new GetContentOptions() { IncludeFields = true, IncludeVersions = true });
            if (contentOpt.IsNone) return contentOpt;
            var content = contentOpt.Get();
            var template = await _templateService.GetByIdAsync(content.TemplateId).MapResultTo(rs => rs.Get());
            var fields = await _templateService.LoadTemplateFields(content.TemplateId);
            content.Template = template;
            content.Template.Fields = fields;
            return content;
        }

        public async Task CreateAsync(Content content)
        {
            await ApplicationPipelines.RunAsync("cms.entity.beforeCreate", new BeforeCreateEntityArg(content));
            var createArg = new BeforeCreateContentArg(content);
            await ApplicationPipelines.RunAsync("cms.content.beforeCreate", createArg);
            await _contentRepository.CreateAsync(await ToEntity(content));
            var moveArg = new GetContentSiblingsOrderArg(content, content.ParentId, MovePosition.Inside);
            createArg.CopyPropertyTo(moveArg);
            await ApplicationPipelines.RunAsync("cms.content.getSiblingsOrder", moveArg);
            await _contentOrderRepository.SaveOrder(moveArg.Orders);
        }

        public async Task UpdateAsync(Content content)
        {
            await ApplicationPipelines.RunAsync("cms.entity.beforeUpdate", new BeforeUpdateEntityArg(content));
            await ApplicationPipelines.RunAsync("cms.content.beforeUpdate", new BeforeUpdateContentArg(content));
            await _contentRepository.UpdateAsync(await ToEntity(content));
        }

        public async Task UpdateExplicitAsync(Content content)
        {
            await ApplicationPipelines.RunAsync("cms.entity.beforeUpdate", new BeforeUpdateEntityArg(content));
            await ApplicationPipelines.RunAsync("cms.content.beforeUpdate", new BeforeUpdateContentArg(content));
            await _contentRepository.UpdateContentExplicit(content.MapTo<ContentEntity>());
        }

        public async Task DeleteAsync(Guid contentId)
        {
            _ = Task.Run(() => _remoteEventHandler.Send(new RemoteEventData("content:deleted", contentId)));
            await _contentRepository.DeleteAsync(contentId);
        }

        public Task NewVersionAsync(Guid contentId, Guid fromVersionId)
            => _contentRepository.CreateNewVersionAsync(contentId, fromVersionId, _sundayContext.CurrentUser!.UserName,
                DateTime.Now);

        public async Task PublishAsync(Guid contentId)
        {
            await _contentRepository.PublishAsync(contentId, _sundayContext.CurrentUser!.UserName, DateTime.Now);
            await _contentLinkService.Save(await GetFullContent(contentId).MapResultTo(c => c.Get()));
            _ = Task.Run(() => _remoteEventHandler.Send(new RemoteEventData("content:published", contentId)));
        }



        public async Task MoveContent(MoveContentParameter moveContentParameter)
        {
            _ = Task.Run(() => _remoteEventHandler.Send(new RemoteEventData("content:moved", moveContentParameter.ContentId)));
            var content = await GetByIdAsync(moveContentParameter.ContentId)
                .MapResultTo(rs => rs.Get());
            if (moveContentParameter.Position == MovePosition.Inside)
            {
                content.ParentId = moveContentParameter.TargetId;
                content.ParentType = moveContentParameter.TargetType;
            }
            else
            {
                var target = await GetByIdAsync(moveContentParameter.TargetId)
                    .MapResultTo(rs => rs.Get());
                content.ParentId = target.ParentId;
                content.ParentType = target.ParentType;
            }
            await UpdateExplicitAsync(content);
            var moveArg = new GetContentSiblingsOrderArg(content, moveContentParameter.TargetId, moveContentParameter.Position);
            await ApplicationPipelines.RunAsync("cms.content.getSiblingsOrder", moveArg);
            await _contentOrderRepository.SaveOrder(moveArg.Orders);
        }

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
