using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Entities;
using Sunday.Core.Models.Base;

namespace Sunday.ContentManagement.Persistence.Application
{
    public interface ITemplateRepository
    {
        Task<SearchResult<TemplateEntity>> QueryAsync(TemplateQuery query);

        Task<Option<TemplateEntity>> GetByIdAsync(Guid templateId);
        Task SaveAsync(TemplateEntity template, SaveTemplateOptions? options = null);
        Task DeleteAsync(Guid templateId);
    }
}
