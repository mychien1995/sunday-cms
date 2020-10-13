using System.Threading.Tasks;
using Sunday.Foundation.Models;

namespace Sunday.Foundation.Application.Services
{
    public interface IAuthenticationService
    {
        Task<SignInResult> PasswordSignInAsync(string username, string password, bool loginToShell = false, bool remember = false);
    }
}
