using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Models;
using Sunday.Core.Models.Base;

namespace Sunday.ContentManagement.Services
{
    public interface IRenderingService
    {
        Task<SearchResult<Rendering>> Search(RenderingQuery query);

        Task<Option<Rendering>> GetRenderingById(Guid id);

        Task Save(Rendering rendering);

        Task Delete(Guid id);
    }
}
