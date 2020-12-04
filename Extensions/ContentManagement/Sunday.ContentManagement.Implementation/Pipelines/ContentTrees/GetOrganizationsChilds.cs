using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Services;
using Sunday.Core.Extensions;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.ContentTrees
{
    public class GetOrganizationsChilds : BaseGetContentTreePipelineProcessor
    {
        public GetOrganizationsChilds(IContentService contentService, IWebsiteService websiteService, IContentOrderRepository contentOrderRepository)
            : base(contentService, websiteService, contentOrderRepository)
        {
        }

        public override async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (GetContentTreeChildrenArg)pipelineArg;
            var node = arg.CurrentNode;
            if (node.Type != (int)ContentType.Organization) return;
            var organizationId = Guid.Parse(node.Id);
            var websites = await WebsiteService.QueryAsync(new WebsiteQuery
            { OrganizationId = organizationId, PageSize = 1000 }).MapResultTo(rs => rs.Result);
            arg.ChildNodes.AddRange(websites.Select(FromWebsite).ToList());
        }
    }
}
