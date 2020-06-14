using Sunday.Core.Domain.Users;
using Sunday.Identity.Core;
using System.Threading.Tasks;

namespace Sunday.Identity.Application
{
    public interface IIdentityService
    {
        Task<SignInResult<T>> PasswordSignInAsync<T>(string username, string password, bool loginToShell = false, bool remember = false) where T : IApplicationUser;

        Task<string> ResetPasswordAsync(int userId);

        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
    }
}
