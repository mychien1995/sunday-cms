using Sunday.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.Application.Identity
{
    public interface IIdentityService
    {
        Task<SignInResult> PasswordSignInAsync(string username, string password, bool loginToShell = false, bool remember = false);

        Task<string> ResetPasswordAsync(int userId);

        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
    }
}
