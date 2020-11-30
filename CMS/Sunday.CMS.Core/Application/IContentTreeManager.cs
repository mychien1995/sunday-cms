using System.Threading.Tasks;
using Sunday.CMS.Core.Models.Contents;
using Sunday.ContentManagement.Models;

namespace Sunday.CMS.Core.Application
{
    public interface IContentTreeManager
    {
        Task<ContentTreeJsonResult> GetRoots();

        Task<ContentTreeListJsonResult> GetChilds(ContentTreeItem current);

        Task<ContextMenuJsonResult> GetContextMenu(ContentTreeItem current);

        Task<ContentTreeJsonResult> GetTreeByPath(string path);

        Task<ContentTreeJsonResult> GetTreeByQuery(ContentTreeQuery query);
    }
}
