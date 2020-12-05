using System;
using System.Threading.Tasks;
using Sunday.CMS.Core.Models.ApplicationWebsites;
using Sunday.ContentManagement.Models;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Application
{
    public interface IApplicationWebsiteManager
    {
        Task<WebsiteListJsonResult> Search(WebsiteQuery criteria);

        Task<BaseApiResponse> Create(WebsiteMutationModel data);

        Task<BaseApiResponse> Update(WebsiteMutationModel data);

        Task<WebsiteDetailJsonResult> GetById(Guid websiteId);

        Task<BaseApiResponse> Delete(Guid websiteId);
        Task<BaseApiResponse> Activate(Guid websiteId);
    }
}
