using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Persistence.Entities;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.ContentManagement.Implementation.Services
{
    [ServiceTypeOf(typeof(ITemplateService))]
    public class DefaultTemplateService : ITemplateService
    {
        private readonly ITemplateRepository _templateRepository;

        public DefaultTemplateService(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public Task<SearchResult<Template>> QueryAsync(TemplateQuery query)
            => _templateRepository.QueryAsync(query).MapResultTo(rs => new SearchResult<Template>
            {
                Total = rs.Total,
                Result = rs.Result.Select(ToDomainModel).ToList()
            });

        public Task<Option<Template>> GetByIdAsync(Guid templateId)
        => _templateRepository.GetByIdAsync(templateId).MapResultTo(rs => rs.Map(ToDomainModel));

        public async Task CreateAsync(Template template)
        {
            await ApplicationPipelines.RunAsync("cms.entity.beforeCreate", new BeforeCreateEntityArg(template));
            await _templateRepository.SaveAsync(ToEntity(template), new SaveTemplateOptions { SaveProperties = true });
        }

        public async Task UpdateAsync(Template template)
        {
            await ApplicationPipelines.RunAsync("cms.entity.beforeUpdate", new BeforeUpdateEntityArg(template));
            await _templateRepository.SaveAsync(ToEntity(template, true), new SaveTemplateOptions { SaveProperties = true });
        }

        public Task DeleteAsync(Guid templateId)
            => _templateRepository.DeleteAsync(templateId);

        private Template ToDomainModel(TemplateEntity entity)
        {
            var template = entity.MapTo<Template>();
            template.Fields = entity.Fields.CastListTo<TemplateField>().ToArray();
            return template;
        }
        private TemplateEntity ToEntity(Template model, bool isUpdate = false)
        {
            var template = model.MapTo<TemplateEntity>();
            template.Fields = model.Fields.CastListTo<TemplateFieldEntity>().ToArray();
            template.IsUpdate = isUpdate;
            return template;
        }
    }
}
