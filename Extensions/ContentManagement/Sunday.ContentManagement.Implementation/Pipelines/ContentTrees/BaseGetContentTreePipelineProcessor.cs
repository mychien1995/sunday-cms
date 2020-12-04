using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Services;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Domain;

namespace Sunday.ContentManagement.Implementation.Pipelines.ContentTrees
{
    public abstract class BaseGetContentTreePipelineProcessor : IAsyncPipelineProcessor
    {
        protected readonly IContentService ContentService;
        protected readonly IWebsiteService WebsiteService;
        protected readonly IContentOrderRepository ContentOrderRepository;

        protected BaseGetContentTreePipelineProcessor(IContentService contentService, IWebsiteService websiteService,
            IContentOrderRepository contentOrderRepository)
        {
            ContentService = contentService;
            WebsiteService = websiteService;
            ContentOrderRepository = contentOrderRepository;
        }

        public abstract Task ProcessAsync(PipelineArg arg);

        protected async Task<Content[]> GetContentChilds(Guid parentId, ContentType contentType)
        {
            var contents = await ContentService.GetChildsAsync(parentId, contentType);
            var orders = await ContentOrderRepository.GetOrders(contents.Select(c => c.Id).ToArray());
            contents.Iter(content =>
            {
                var order = orders.FirstOrDefault(o => o.ContentId == content.Id);
                if (order != null) content.SortOrder = order.Order;
            });
            return contents.OrderBy(c => c.SortOrder).ThenBy(c => c.Name).ToArray();
        }

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

        protected ContentTreeNode FromContent(Content content, string? parentId = null)
            => new ContentTreeNode
            {
                Name = content.DisplayName,
                Icon = content.TemplateId.ToString(),
                Id = content.Id.ToString(),
                Link = $"/manage-contents/{content.Id}",
                Type = (int)ContentType.Content,
                ParentId = parentId,
                SortOrder = content.SortOrder
            };
    }
}
