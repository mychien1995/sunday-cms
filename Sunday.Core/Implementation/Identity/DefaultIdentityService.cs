using Sunday.Core.Domain.Identity;
using Sunday.Core.Application.Identity;
using Sunday.Core.Pipelines.Arguments;
using Sunday.Core.Ultilities;
using Sunday.Core.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.Implementation.Identity
{
    [ServiceTypeOf(typeof(IIdentityService))]
    public class DefaultIdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        public DefaultIdentityService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<SignInResult> PasswordSignInAsync(string username, string password, bool loginToShell = false, bool remember = false)
        {
            LoginStatus status = LoginStatus.Failure;
            var user = await _userRepository.FindUserByNameAsync(username);
            if (user == null || user.IsDeleted) status = LoginStatus.Failure;
            else if (!user.IsActive || user.IsLockedOut) status = LoginStatus.LockedOut;
            else
            {
                var hashed = EncryptUltis.SHA256Encrypt(password, user.SecurityStamp);
                if (hashed != user.PasswordHash) status = LoginStatus.Failure;
                else if (!user.EmailConfirmed) status = LoginStatus.RequiresVerification;
                else if (user.Domain != DomainNames.Shell && loginToShell) status = LoginStatus.Failure;
                else status = LoginStatus.Success;
            }
            if (status == LoginStatus.Success)
                return new SignInResult(status, user);
            return new SignInResult(status);
        }

        public async Task<string> ResetPasswordAsync(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            var securityHash = Guid.NewGuid().ToString("N");
            var newPassword = PasswordUltils.RandomString(6);
            var passwordHash = EncryptUltis.SHA256Encrypt(newPassword, user.SecurityStamp);
            user.SecurityStamp = securityHash;
            user.PasswordHash = passwordHash;
            await _userRepository.UpdatePassword(user);
            return newPassword;
        }
    }
}
