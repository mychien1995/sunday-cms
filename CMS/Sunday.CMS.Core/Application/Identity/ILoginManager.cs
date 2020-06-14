using Sunday.CMS.Core.Models;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Application.Identity
{
    public interface ILoginManager
    {
        Task<LoginApiResponse> LoginAsync(LoginInputModel credential);
    }
}
