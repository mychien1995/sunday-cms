using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Domain;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Models;

namespace Sunday.ContentManagement.Services
{
    public interface ILayoutService
    {
        Task<SearchResult<ApplicationLayout>> QueryAsync(LayoutQuery query);
        Task<Option<ApplicationLayout>> GetByIdAsync(Guid layoutId);

        Task CreateAsync(ApplicationLayout layout);
        Task UpdateAsync(ApplicationLayout layout);
        Task DeleteAsync(Guid layoutId);
    }
}
