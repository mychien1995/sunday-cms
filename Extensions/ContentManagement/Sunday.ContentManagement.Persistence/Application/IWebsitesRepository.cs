using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Entities;
using Sunday.Core.Models.Base;

namespace Sunday.ContentManagement.Persistence.Application
{
    public interface IWebsitesRepository
    {
        Task<SearchResult<WebsiteEntity>> QueryAsync(WebsiteQuery query);
        Task<Option<WebsiteEntity>> GetByIdAsync(Guid websiteId);

        Task CreateAsync(WebsiteEntity website);
        Task UpdateAsync(WebsiteEntity website);
        Task DeleteAsync(Guid websiteId);
    }
}
