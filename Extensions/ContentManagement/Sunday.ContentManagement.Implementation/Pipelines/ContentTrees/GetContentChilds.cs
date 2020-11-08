using System;
using System.Threading.Tasks;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Services;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.ContentTrees
{
    public class GetContentChilds : BaseGetContentTreePipelineProcessor
    {
        private readonly IContentService _contentService;

        public GetContentChilds(IContentService contentService)
        {
            _contentService = contentService;
        }

        public override async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (GetContentTreeChildrenArg)pipelineArg;
            var node = arg.CurrentNode;
            if (node.Type != (int)ContentType.Content) return;
            var contents = await _contentService.GetChildsAsync(Guid.Parse(node.Id), ContentType.Content);
            contents.Iter(content =>
            {
                arg.ChildNodes.Add(FromContent(content));
            });
        }
    }
}