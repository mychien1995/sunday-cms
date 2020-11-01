using System;
using System.Threading.Tasks;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Services
{
    public class DefaultContentTreeProvider : IContentTreeProvider
    {
        public async Task<ContentTree> GetTreeRoot()
        {
            var arg = new GetContentTreeRootArg();
            await ApplicationPipelines.RunAsync("cms.contentTree.getRoots", arg);
            return new ContentTree { Roots = arg.Roots.ToArray() };
        }

        public async Task<ContentTreeNode[]> GetChildren(ContentTreeNode currentNode)
        {
            var arg = new GetContentTreeChildrenArg(currentNode);
            await ApplicationPipelines.RunAsync("cms.contentTree.getChilds", arg);
            return arg.ChildNodes.ToArray();
        }
    }
}
