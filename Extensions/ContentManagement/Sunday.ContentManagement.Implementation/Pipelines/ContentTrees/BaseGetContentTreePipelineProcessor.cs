using System.Threading.Tasks;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Models;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Domain;

namespace Sunday.ContentManagement.Implementation.Pipelines.ContentTrees
{
    public abstract class BaseGetContentTreePipelineProcessor : IAsyncPipelineProcessor
    {
        public abstract Task ProcessAsync(PipelineArg arg);

        protected ContentTreeNode FromOrganization(ApplicationOrganization organization)
            => new ContentTreeNode
            {
                Name = organization!.OrganizationName,
                Icon = Constants.NodeIcons.Organization,
                Id = organization!.Id.ToString(),
                Link = "#",
                Type = (int)ContentType.Organization
            };
        protected ContentTreeNode FromWebsite(ApplicationWebsite website)
            => new ContentTreeNode
            {
                Name = website.WebsiteName,
                Icon = Constants.NodeIcons.Website,
                Id = website.Id.ToString(),
                Link = "#",
                Type = (int)ContentType.Website
            };

        protected ContentTreeNode FromContent(Content content)
            => new ContentTreeNode
            {
                Name = content.DisplayName,
                Icon = content.TemplateId.ToString(),
                Id = content.Id.ToString(),
                Link = "#",
                Type = (int)ContentType.Content
            };
    }
}
