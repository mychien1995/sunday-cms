using System.Threading.Tasks;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Services;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Context;

namespace Sunday.ContentManagement.Implementation.Pipelines.ContentTrees
{
    public class GetRoots : BaseGetTreeRootPipelineProcessor
    {

        public GetRoots(ISundayContext sundayContext, IOrganizationService organizationService, IWebsiteService websiteService, IContentOrderRepository contentOrderRepository,
            IContentService contentService) : 
            base(sundayContext, organizationService, websiteService, contentOrderRepository, contentService)
        {
        }

        public override async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (GetContentTreeRootArg)pipelineArg;
            var roots = arg.Roots;
            roots.AddRange((await GetTreeRoot()).Roots);
        }

        
    }
}
