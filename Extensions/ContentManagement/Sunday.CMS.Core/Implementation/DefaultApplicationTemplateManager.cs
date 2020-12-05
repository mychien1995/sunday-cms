using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Templates;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Implementation
{
    [ServiceTypeOf(typeof(IApplicationTemplateManager))]
    public class DefaultApplicationTemplateManager : IApplicationTemplateManager
    {
        private readonly ITemplateService _templateService;
        private readonly IFieldTypesProvider _fieldTypesLoader;
        private readonly IEntityAccessService _entityAccessService;

        public DefaultApplicationTemplateManager(ITemplateService templateService, IFieldTypesProvider fieldTypesLoader, IEntityAccessService entityAccessService)
        {
            _templateService = templateService;
            _fieldTypesLoader = fieldTypesLoader;
            _entityAccessService = entityAccessService;
        }

        public Task<TemplateListJsonResult> Search(TemplateQuery criteria)
        => _templateService.QueryAsync(criteria).MapResultTo(rs => new TemplateListJsonResult
        {
            Total = rs.Total,
            Templates = rs.Result.Select(ToModel).ToList()
        });

        public async Task<BaseApiResponse> Create(TemplateItem data)
        {
            var template = await _templateService.CreateAsync(ToDomainModel(data));
            data.Id = template.Id;
            if (data.Access != null)
                await _entityAccessService.Save(GetEntityAccess(data));
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> Update(TemplateItem data)
        {
            await _templateService.UpdateAsync(ToDomainModel(data));
            if (data.Access != null)
                await _entityAccessService.Save(GetEntityAccess(data));
            return BaseApiResponse.SuccessResult;
        }

        public async Task<TemplateItem> GetById(Guid templateId)
        {
            var templateOpt = await _templateService.GetByIdAsync(templateId).MapResultTo(rs => rs.Map(ToModel));
            if (templateOpt.IsNone) return BaseApiResponse.ErrorResult<TemplateItem>("Template not found");
            var template = templateOpt.Get();
            var access = await _entityAccessService.GetByEntity(templateId, nameof(Template));
            access.IfSome(acc => template.Access = acc);
            return template;
        }


        public async Task<BaseApiResponse> Delete(Guid templateId)
        {
            await _templateService.DeleteAsync(templateId);
            return BaseApiResponse.SuccessResult;
        }

        public FieldTypeListJsonResult GetFieldTypes()
            => new FieldTypeListJsonResult()
            {
                FieldTypes = _fieldTypesLoader.List().OrderBy(f => f.Id).Select(f => new FieldTypeItem { Id = f.Id, Name = f.Name, Layout = f.Layout })
                    .ToArray()
            };

        public Task<BaseApiResponse<TemplateFieldItem[]>> LoadTemplateFields(Guid templateId)
            => _templateService.LoadTemplateFields(templateId).MapResultTo(rs =>
                new BaseApiResponse<TemplateFieldItem[]>(rs.Select(ToModel).ToArray()));

        private TemplateItem ToModel(Template template)
        {
            var item = template.MapTo<TemplateItem>();
            return item;
        }

        private Template ToDomainModel(TemplateItem template)
        {
            var model = template.MapTo<Template>();
            return model;
        }

        private TemplateFieldItem ToModel(TemplateField template)
        {
            var item = template.MapTo<TemplateFieldItem>();
            return item;
        }

        private EntityAccess GetEntityAccess(TemplateItem template)
        {
            var access = template.Access!;
            access.EntityType = nameof(Template);
            access.EntityId = template.Id;
            return access;
        }


    }
}
