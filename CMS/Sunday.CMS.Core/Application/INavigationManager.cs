using System.Threading.Tasks;
using Sunday.CMS.Core.Models.Layout;

namespace Sunday.CMS.Core.Application
{
    public interface INavigationManager
    {
        Task<NavigationTreeResponse> GetUserNavigation();
    }
}
