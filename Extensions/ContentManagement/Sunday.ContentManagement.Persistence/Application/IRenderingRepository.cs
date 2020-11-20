using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Entities;
using Sunday.Core.Models.Base;

namespace Sunday.ContentManagement.Persistence.Application
{
    public interface IRenderingRepository
    {
        Task<SearchResult<RenderingEntity>> Search(RenderingQuery query);

        Task<Option<RenderingEntity>> GetRenderingById(Guid id);

        Task Save(RenderingEntity rendering);

        Task Delete(Guid id);
    }
}
