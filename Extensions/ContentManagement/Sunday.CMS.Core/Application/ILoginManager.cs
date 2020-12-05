using System.Threading.Tasks;
using Sunday.CMS.Core.Models;
using Sunday.CMS.Core.Models.Login;

namespace Sunday.CMS.Core.Application
{
    public interface ILoginManager
    {
        Task<LoginApiResponse> LoginAsync(LoginInputModel credential);
    }
}
