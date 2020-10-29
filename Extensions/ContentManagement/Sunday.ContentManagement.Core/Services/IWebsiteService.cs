using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Models;
using Sunday.Core.Models.Base;

namespace Sunday.ContentManagement.Services
{
    public interface IWebsiteService
    {
        Task<SearchResult<ApplicationWebsite>> QueryAsync(WebsiteQuery query);
        Task<Option<ApplicationWebsite>> GetByIdAsync(Guid websiteId);

        Task CreateAsync(ApplicationWebsite website);
        Task UpdateAsync(ApplicationWebsite website);
        Task DeleteAsync(Guid websiteId);
    }
}
