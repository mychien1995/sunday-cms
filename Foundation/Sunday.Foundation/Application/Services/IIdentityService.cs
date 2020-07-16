using System.Threading.Tasks;
using Sunday.Core.Domain.Users;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Models;

namespace Sunday.Foundation.Application.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> AuthenticateByPasswordAsync(string username, string password,
            bool loginToShell = false, bool remember = false);
        Task<SignInResult> PasswordSignInAsync(string username, string password, bool loginToShell = false, bool remember = false);

        Task<string> ResetPasswordAsync(int userId);

        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
    }
}
