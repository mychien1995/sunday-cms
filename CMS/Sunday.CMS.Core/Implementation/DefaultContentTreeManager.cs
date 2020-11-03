using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Contents;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Extensions;

namespace Sunday.CMS.Core.Implementation
{
    [ServiceTypeOf(typeof(IContentTreeManager))]
    public class DefaultContentTreeManager : IContentTreeManager
    {
        private readonly IContentTreeProvider _contentTreeProvider;

        public DefaultContentTreeManager(IContentTreeProvider contentTreeProvider)
        {
            _contentTreeProvider = contentTreeProvider;
        }

        public Task<ContentTreeJsonResult> GetRoots()
            => _contentTreeProvider.GetTreeRoot().MapResultTo(rs => new ContentTreeJsonResult
            { Roots = rs.Roots.Select(ToItem).ToArray() });

        public Task<ContentTreeListJsonResult> GetChilds(ContentTreeItem current)
            => _contentTreeProvider.GetChildren(current.MapTo<ContentTreeNode>())
                .MapResultTo(rs => new ContentTreeListJsonResult() { Nodes = rs.Select(ToItem).ToArray() });

        private ContentTreeItem ToItem(ContentTreeNode node)
        {
            var item = node.MapTo<ContentTreeItem>();
            item.ChildNodes = node.ChildNodes.CastListTo<ContentTreeItem>().ToArray();
            return item;
        }
    }
}
