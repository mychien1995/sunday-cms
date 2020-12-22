using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.CacheDependencies
{
    public class GetContentDependants : IAsyncPipelineProcessor
    {
        private readonly IContentLinkService _contentLinkService;
        private readonly ICacheKeyCreator _cacheKeyCreator;

        public GetContentDependants(IContentLinkService contentLinkService, ICacheKeyCreator cacheKeyCreator)
        {
            _contentLinkService = contentLinkService;
            _cacheKeyCreator = cacheKeyCreator;
        }
        public async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (GetEntityDependantsArg)pipelineArg;
            if (arg.Entity.GetType() != typeof(Content)) return;
            var content = (arg.Entity as Content)!;
            var dependants = new List<string>();
            var referencesFrom = await _contentLinkService.GetReferencesTo(content.Id);
            dependants.AddRange(referencesFrom.Select(r => _cacheKeyCreator.GetCacheKey(typeof(Content), r)));
            arg.Dependants.AddRange(dependants.Distinct());
        }
    }
}
