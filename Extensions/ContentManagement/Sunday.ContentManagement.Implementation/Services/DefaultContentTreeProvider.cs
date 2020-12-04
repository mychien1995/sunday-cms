using System.Threading.Tasks;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Services
{
    [ServiceTypeOf(typeof(IContentTreeProvider))]
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

        public async Task<ContextMenu> GetContextMenu(ContentTreeNode currentNode)
        {
            var arg = new GetContextMenuArg(currentNode);
            await ApplicationPipelines.RunAsync("cms.contentTree.getContextMenu", arg);
            return arg.Menu;
        }

        public async Task<ContentTree> GetTreeSnapshotByPath(string path)
        {
            var arg = new GetContentTreeSnapshotArg(path);
            await ApplicationPipelines.RunAsync("cms.contentTree.getByPath", arg);
            return arg.ContentTree;
        }

        public async Task<ContentTree> GetTreeSnapshotByQuery(ContentTreeQuery contentTreeQuery)
        {
            var arg = new GetContentTreeByQueryArg(contentTreeQuery);
            await ApplicationPipelines.RunAsync("cms.contentTree.getByQuery", arg);
            return arg.ContentTree!;
        }
    }
}
