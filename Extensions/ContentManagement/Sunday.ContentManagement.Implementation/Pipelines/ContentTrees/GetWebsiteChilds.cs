using System;
using System.Threading.Tasks;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Services;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.ContentTrees
{
    public class GetWebsiteChilds : BaseGetContentTreePipelineProcessor
    {
        private readonly IContentService _contentService;

        public GetWebsiteChilds(IContentService contentService)
        {
            _contentService = contentService;
        }

        public override async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (GetContentTreeChildrenArg)pipelineArg;
            var node = arg.CurrentNode;
            if (node.Type != (int)ContentType.Website) return;
            var contents = await _contentService.GetChildsAsync(Guid.Parse(node.Id), ContentType.Website);
            contents.Iter(content =>
            {
                arg.ChildNodes.Add(FromContent(content));
            });
        }
    }
}
