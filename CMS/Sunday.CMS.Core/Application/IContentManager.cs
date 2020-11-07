using System;
using System.Threading.Tasks;
using Sunday.CMS.Core.Models.Contents;

namespace Sunday.CMS.Core.Application
{
    public interface IContentManager
    {
        Task<ContentJsonResult> GetContentByIdAsync(Guid contentId, Guid? versionId = null);

        Task CreateContentAsync(ContentJsonResult content);
        Task UpdateContentAsync(ContentJsonResult content);
        Task DeleteContentAsync(Guid contentId);
        Task NewContentVersionAsync(Guid contentId, Guid fromVersion);
        Task PublishContentAsync(Guid contentId);
    }
}
