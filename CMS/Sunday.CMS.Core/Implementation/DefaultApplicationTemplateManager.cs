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

namespace Sunday.CMS.Core.Implementation
{
    [ServiceTypeOf(typeof(IApplicationTemplateManager))]
    public class DefaultApplicationTemplateManager : IApplicationTemplateManager
    {
        private readonly ITemplateService _templateService;
        private readonly IFieldTypesLoader _fieldTypesLoader;

        public DefaultApplicationTemplateManager(ITemplateService templateService, IFieldTypesLoader fieldTypesLoader)
        {
            _templateService = templateService;
            _fieldTypesLoader = fieldTypesLoader;
        }

        public Task<TemplateListJsonResult> Search(TemplateQuery criteria)
        => _templateService.QueryAsync(criteria).MapResultTo(rs => new TemplateListJsonResult
        {
            Total = rs.Total,
            Templates = rs.Result.Select(ToModel).ToList()
        });

        public async Task<BaseApiResponse> Create(TemplateItem data)
        {
            await _templateService.CreateAsync(ToDomainModel(data));
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> Update(TemplateItem data)
        {
            await _templateService.UpdateAsync(ToDomainModel(data));
            return BaseApiResponse.SuccessResult;
        }

        public Task<TemplateItem> GetById(Guid templateId)
        => _templateService.GetByIdAsync(templateId).MapResultTo(rs => rs.Map(ToModel).IfNone(BaseApiResponse
            .ErrorResult<TemplateItem>("Template not found")));


        public async Task<BaseApiResponse> Delete(Guid templateId)
        {
            await _templateService.DeleteAsync(templateId);
            return BaseApiResponse.SuccessResult;
        }

        public FieldTypeListJsonResult GetFieldTypes()
            => new FieldTypeListJsonResult()
            {
                FieldTypes = _fieldTypesLoader.List().Select(f => new FieldTypeItem { Id = f.Id, Name = f.Name })
                    .ToArray()
            };

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
    }
}
