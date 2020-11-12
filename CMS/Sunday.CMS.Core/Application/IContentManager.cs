using System;
using System.Threading.Tasks;
using Sunday.CMS.Core.Models.Contents;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Application
{
    public interface IContentManager
    {
        Task<ContentJsonResult> GetContentByIdAsync(Guid contentId, Guid? versionId = null);

        Task<CreateContentJsonResult> CreateContentAsync(ContentJsonResult content);
        Task<BaseApiResponse> UpdateContentAsync(ContentJsonResult content);
        Task<BaseApiResponse> DeleteContentAsync(Guid contentId);
        Task<BaseApiResponse> NewContentVersionAsync(Guid contentId, Guid fromVersion);
        Task<BaseApiResponse> PublishContentAsync(Guid contentId);
    }
}
