using Sunday.CMS.Core.Models.Layout;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Application.Layout
{
    public interface INavigationManager
    {
        Task<NavigationTreeResponse> GetUserNavigation();
    }
}
