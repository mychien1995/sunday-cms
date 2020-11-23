using System;
using System.Threading.Tasks;
using Sunday.CMS.Core.Models.Contents;
using Sunday.ContentManagement.Models;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Application
{
    public interface IRenderingManager
    {
        Task<ListApiResponse<RenderingJsonResult>> Search(RenderingQuery query);

        Task<RenderingJsonResult> GetById(Guid id);

        Task<BaseApiResponse> Create(RenderingJsonResult rendering);
        Task<BaseApiResponse> Update(RenderingJsonResult rendering);

        Task<BaseApiResponse> Delete(Guid id);
    }
}
