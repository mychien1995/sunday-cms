using System;
using System.Threading.Tasks;
using Sunday.Foundation.Models;

namespace Sunday.Foundation.Application.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> AuthenticateAsync(string username, string password, bool loginToShell = false, bool remember = false);

        Task<string> ResetPasswordAsync(Guid userId);

        Task<bool> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
    }
}
