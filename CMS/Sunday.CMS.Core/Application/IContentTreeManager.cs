using System.Threading.Tasks;
using Sunday.CMS.Core.Models.Contents;

namespace Sunday.CMS.Core.Application
{
    public interface IContentTreeManager
    {
        Task<ContentTreeJsonResult> GetRoots();

        Task<ContentTreeListJsonResult> GetChilds(ContentTreeItem current);

        Task<ContextMenuJsonResult> GetContextMenu(ContentTreeItem current);
    }
}
