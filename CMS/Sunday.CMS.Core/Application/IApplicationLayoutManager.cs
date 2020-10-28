using System;
using System.Threading.Tasks;
using Sunday.CMS.Core.Models.ApplicationLayouts;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Models;

namespace Sunday.CMS.Core.Application
{
    public interface IApplicationLayoutManager
    {
        Task<LayoutListJsonResult> SearchLayout(LayoutQuery criteria);

        Task<BaseApiResponse> CreateLayout(LayoutMutationModel data);

        Task<BaseApiResponse> UpdateLayout(LayoutMutationModel data);

        Task<LayoutDetailJsonResult> GetLayoutById(Guid layoutId);

        Task<BaseApiResponse> DeleteLayout(Guid layoutId);
    }
}
