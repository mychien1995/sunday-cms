using Sunday.ContentManagement.Models;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Arguments
{
    public class GetContextMenuArg :  PipelineArg
    {
        public GetContextMenuArg(ContentTreeNode currentNode)
        {
            CurrentNode = currentNode;
        }

        public ContentTreeNode CurrentNode { get; set; }
    }
}
