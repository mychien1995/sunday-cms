using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Models;
using Sunday.Core.Models.Base;

namespace Sunday.ContentManagement.Services
{
    public interface ITemplateService
    {
        Task<SearchResult<Template>> QueryAsync(TemplateQuery query);
        Task<Option<Template>> GetByIdAsync(Guid templateId);

        Task<Template> CreateAsync(Template template);

        Task UpdateAsync(Template template);

        Task DeleteAsync(Guid templateId);
        Task<TemplateField[]> LoadTemplateFields(Guid templateId);
    }
}
