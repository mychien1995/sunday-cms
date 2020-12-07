using System.Threading.Tasks;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Services;
using Sunday.Core.Extensions;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Context;

namespace Sunday.ContentManagement.Implementation.Pipelines.ContentTrees
{
    public class GetContentTreeByQuery : BaseGetTreeRootPipelineProcessor
    {
        private readonly IContentPathResolver _contentPathResolver;
        private readonly IContentTreeProvider _contentTreeProvider;
        public GetContentTreeByQuery(ISundayContext sundayContext, IOrganizationService organizationService, IWebsiteService websiteService,
            IContentPathResolver contentPathResolver, IContentTreeProvider contentTreeProvider, IContentService contentService, IContentOrderRepository contentOrderRepository)
            : base(sundayContext, organizationService, websiteService, contentOrderRepository, contentService)
        {
            _contentPathResolver = contentPathResolver;
            _contentTreeProvider = contentTreeProvider;
        }

        public override async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var tree = await GetTreeRoot();
            var arg = (GetContentTreeByQueryArg)pipelineArg;
            arg.ContentTree = tree;
            var location = arg.ContentTreeQuery.Location;
            var websiteOpt = await WebsiteService.GetByIdAsync(arg.ContentTreeQuery.WebsiteId);
            if (websiteOpt.IsNone) return;
            var website = websiteOpt.Get();
            location ??= string.Empty;
            if (location.StartsWith("$site"))
            {
                location = location.Replace("$site", string.Empty);
            }
            var contentOpt = await _contentPathResolver.GetContentByNamePath(website.Id, location);
            if (contentOpt.IsNone) return;
            arg.ContentTree = await _contentTreeProvider.GetTreeSnapshotByPath(contentOpt.Get().Path);
        }
    }
}
