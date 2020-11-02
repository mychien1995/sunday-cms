using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core.Extensions;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.ContentTrees
{
    public class GetOrganizationsChilds : BaseGetContentTreePipelineProcessor
    {
        private readonly IWebsiteService _websiteService;

        public GetOrganizationsChilds(IWebsiteService websiteService)
        {
            _websiteService = websiteService;
        }

        public override async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (GetContentTreeChildrenArg)pipelineArg;
            var node = arg.CurrentNode;
            if (node.Type != Constants.NodeTypes.Organization) return;
            var organizationId = Guid.Parse(node.Id);
            var websites = await _websiteService.QueryAsync(new WebsiteQuery
            { OrganizationId = organizationId, PageSize = 1000 }).MapResultTo(rs => rs.Result);
            node.ChildNodes.AddRange(websites.Select(FromWebsite).ToList());
        }
    }
}
