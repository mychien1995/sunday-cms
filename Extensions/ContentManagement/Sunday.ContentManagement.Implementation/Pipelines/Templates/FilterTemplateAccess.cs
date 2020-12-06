using System.Threading.Tasks;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Services;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Context;

namespace Sunday.ContentManagement.Implementation.Pipelines.Templates
{
    public class FilterTemplateAccess : BaseFilterEntityAccessProcessor<Template>
    {
        public FilterTemplateAccess(IEntityAccessService entityAccessService, ISundayContext sundayContext, IWebsiteService websiteService)
            : base(entityAccessService, sundayContext, websiteService)
        {
        }

        public override async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (FilterTemplatesArg)pipelineArg;
            var filteredTemplates = await FilterEntities(arg.Result.Result, arg.Query.OrganizationId, arg.Query.WebsiteId, nameof(Template));
            arg.Result.Result = filteredTemplates;
            arg.Result.Total = filteredTemplates.Length;
        }
    }
}
