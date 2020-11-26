using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Persistence.Entities;
using Sunday.ContentManagement.Persistence.Models;
using Sunday.Core.Models.Base;

namespace Sunday.ContentManagement.Persistence.Application
{
    public interface IRenderingRepository
    {
        Task<SearchResult<RenderingEntity>> Search(RenderingQueryParameter query);

        Task<Option<RenderingEntity>> GetRenderingById(Guid id);

        Task Save(RenderingEntity rendering);

        Task Delete(Guid id);
    }
}
