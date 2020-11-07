using System;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Contents;
using Sunday.Core;

namespace Sunday.CMS.Core.Implementation
{
    [ServiceTypeOf(typeof(IContentManager))]
    public class DefaultContentManager : IContentManager
    {
        public Task<ContentJsonResult> GetContentByIdAsync(Guid contentId, Guid? versionId = null)
        {
            throw new NotImplementedException();
        }

        public Task CreateContentAsync(ContentJsonResult content)
        {
            throw new NotImplementedException();
        }

        public Task UpdateContentAsync(ContentJsonResult content)
        {
            throw new NotImplementedException();
        }

        public Task DeleteContentAsync(Guid contentId)
        {
            throw new NotImplementedException();
        }

        public Task NewContentVersionAsync(Guid contentId, Guid fromVersion)
        {
            throw new NotImplementedException();
        }

        public Task PublishContentAsync(Guid contentId)
        {
            throw new NotImplementedException();
        }
    }
}
