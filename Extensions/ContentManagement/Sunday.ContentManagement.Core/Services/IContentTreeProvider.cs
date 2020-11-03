using System.Threading.Tasks;
using Sunday.ContentManagement.Models;

namespace Sunday.ContentManagement.Services
{
    public interface IContentTreeProvider
    {
        Task<ContentTree> GetTreeRoot();

        Task<ContentTreeNode[]> GetChildren(ContentTreeNode currentNode);

        Task<ContextMenu> GetContextMenu(ContentTreeNode currentNode);
    }
}
