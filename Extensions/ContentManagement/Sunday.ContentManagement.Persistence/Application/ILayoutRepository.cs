using System;
using System.Threading.Tasks;
using Sunday.ContentManagement.Persistence.Entities;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Models;

namespace Sunday.ContentManagement.Persistence.Application
{
    public interface ILayoutRepository
    {
        Task<SearchResult<LayoutEntity>> QueryAsync(LayoutQuery query);

        Task CreateAsync(LayoutEntity layout);
        Task UpdateAsync(LayoutEntity layout);
        Task DeleteAsync(Guid layoutId);
    }
}
