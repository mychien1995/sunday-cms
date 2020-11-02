using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core.Extensions;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Context;
using Sunday.Foundation.Models;

namespace Sunday.ContentManagement.Implementation.Pipelines.ContentTrees
{
    public class GetRoots : BaseGetContentTreePipelineProcessor
    {
        private readonly ISundayContext _sundayContext;
        private readonly IOrganizationService _organizationService;
        private readonly IWebsiteService _websiteService;

        public GetRoots(ISundayContext sundayContext, IOrganizationService organizationService, IWebsiteService websiteService)
        {
            _sundayContext = sundayContext;
            _organizationService = organizationService;
            _websiteService = websiteService;
        }

        public override async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (GetContentTreeRootArg)pipelineArg;
            var roots = arg.Roots;
            var currentUser = _sundayContext.CurrentUser;
            if (currentUser!.IsOrganizationMember())
            {
                var organization = _sundayContext.CurrentOrganization;
                var node = FromOrganization(organization!);
                var websites = await _websiteService.QueryAsync(new WebsiteQuery
                { OrganizationId = organization!.Id, PageSize = 1000 }).MapResultTo(rs => rs.Result);
                node.ChildNodes = websites.Select(FromWebsite).ToList();
                roots.Add(node);
            }
            else
            {
                var organizations = await _organizationService
                    .QueryAsync(new OrganizationQuery { IsActive = true, PageSize = 1000 }).MapResultTo(rs => rs.Result);
                roots.AddRange(organizations.Select(FromOrganization));
                roots.Iter(async node =>
                {
                    var websites = await _websiteService.QueryAsync(new WebsiteQuery
                    { OrganizationId = Guid.Parse(node.Id), PageSize = 1000 }).MapResultTo(rs => rs.Result);
                    node.ChildNodes = websites.Select(FromWebsite).ToList();
                });
            }
        }

        
    }
}
