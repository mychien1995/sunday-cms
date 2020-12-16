using System.Linq;
using System.Threading.Tasks;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Services;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.CacheDependencies
{
    public class GetTemplateMasters : IAsyncPipelineProcessor
    {
        private readonly ITemplateService _templateService;
        private readonly ICacheKeyCreator _cacheKeyCreator;

        public GetTemplateMasters(ITemplateService templateService, ICacheKeyCreator cacheKeyCreator)
        {
            _templateService = templateService;
            _cacheKeyCreator = cacheKeyCreator;
        }

        public async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (GetEntityMastersArg)pipelineArg;
            if (arg.Entity.GetType() != typeof(Template)) return;
            var template = (arg.Entity as Template)!;
            var ancestor =  await _templateService.GetAncestors(template.Id);
            arg.Masters.AddRange(ancestor.Select(t => _cacheKeyCreator.GetCacheKey(typeof(Template), t.Id)));
        }
    }
}
