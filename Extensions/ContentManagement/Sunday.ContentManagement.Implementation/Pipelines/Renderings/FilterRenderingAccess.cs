using System.Threading.Tasks;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Context;

namespace Sunday.ContentManagement.Implementation.Pipelines.Renderings
{
    public class FilterRenderingAccess : BaseFilterEntityAccessProcessor<Rendering>
    {
        public FilterRenderingAccess(IEntityAccessService entityAccessService, ISundayContext sundayContext, IWebsiteService websiteService) 
            : base(entityAccessService, sundayContext, websiteService)
        {
        }
        public override async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (FilterRenderingsArgs)pipelineArg;
            var filteredRenderings =  await FilterEntities(arg.Result.Result, arg.Query.WebsiteId, nameof(Rendering));
            arg.Result.Result = filteredRenderings;
            arg.Result.Total = filteredRenderings.Length;
        }

        
    }
}
