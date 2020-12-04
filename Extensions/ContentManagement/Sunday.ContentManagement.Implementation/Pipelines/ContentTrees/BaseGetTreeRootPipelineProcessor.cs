using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Services;
using Sunday.Core.Extensions;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Context;
using Sunday.Foundation.Models;

namespace Sunday.ContentManagement.Implementation.Pipelines.ContentTrees
{
    public abstract class BaseGetTreeRootPipelineProcessor : BaseGetContentTreePipelineProcessor
    {
        protected readonly ISundayContext SundayContext;
        protected readonly IOrganizationService OrganizationService;

        protected BaseGetTreeRootPipelineProcessor(ISundayContext sundayContext, IOrganizationService organizationService, IWebsiteService websiteService
        , IContentOrderRepository contentOrderRepository, IContentService contentService) : base(contentService, websiteService, contentOrderRepository)
        {
            SundayContext = sundayContext;
            OrganizationService = organizationService;
        }
        protected async Task<ContentTree> GetTreeRoot()
        {
            var contentTree= new ContentTree();
            var roots = new List<ContentTreeNode>();
            var currentUser = SundayContext.CurrentUser;
            if (currentUser!.IsOrganizationMember())
            {
                var organization = SundayContext.CurrentOrganization;
                var node = FromOrganization(organization!);
                var websites = await WebsiteService.QueryAsync(new WebsiteQuery
                    { OrganizationId = organization!.Id, PageSize = 1000 }).MapResultTo(rs => rs.Result);
                node.ChildNodes = websites.Select(FromWebsite).ToList();
                roots.Add(node);
            }
            else
            {
                var organizations = await OrganizationService
                    .QueryAsync(new OrganizationQuery { IsActive = true, PageSize = 1000 }).MapResultTo(rs => rs.Result);
                roots.AddRange(organizations.Select(FromOrganization));
                foreach (var node in roots)
                {
                    var websites = await WebsiteService.QueryAsync(new WebsiteQuery
                        { OrganizationId = Guid.Parse(node.Id), PageSize = 1000 }).MapResultTo(rs => rs.Result);
                    node.ChildNodes = websites.Select(FromWebsite).ToList();

                }
            }

            contentTree.Roots = roots.ToArray();
            return contentTree;
        }
    }
}
