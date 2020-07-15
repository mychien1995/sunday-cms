using Sunday.Core;
using Sunday.Core.Domain.Users;
using Sunday.Core.Ultilities;
using Sunday.Identity.Application;
using Sunday.Identity.Core;
using Sunday.Users.Application;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sunday.Identity.Implementation
{
    [ServiceTypeOf(typeof(IIdentityService))]
    public class DefaultIdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        public DefaultIdentityService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async virtual Task<SignInResult<T>> PasswordSignInAsync<T>(string username, string password, bool loginToShell = false, bool remember = false)
            where T : IApplicationUser
        {
            LoginStatus status = LoginStatus.Failure;
            var user = await _userRepository.FindUserByNameAsync(username);
            if (user == null || user.IsDeleted) status = LoginStatus.Failure;
            else if (!user.IsActive || user.IsLockedOut) status = LoginStatus.LockedOut;
            else
            {
                var hashed = EncryptUltis.Sha256Encrypt(password, user.SecurityStamp);
                if (hashed != user.PasswordHash) status = LoginStatus.Failure;
                else if (!user.EmailConfirmed) status = LoginStatus.RequiresVerification;
                else if (user.Domain != DomainNames.Shell && loginToShell) status = LoginStatus.Failure;
                else status = LoginStatus.Success;
            }
            if (status == LoginStatus.Success)
                return new SignInResult<T>(status, user);
            return new SignInResult<T>(status);
        }

        public async virtual Task<string> ResetPasswordAsync(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            var securityHash = Guid.NewGuid().ToString("N");
            var newPassword = PasswordUltils.RandomString(6);
            var passwordHash = EncryptUltis.Sha256Encrypt(newPassword, user.SecurityStamp);
            user.SecurityStamp = securityHash;
            user.PasswordHash = passwordHash;
            await _userRepository.UpdatePassword(user);
            return newPassword;
        }

        public async virtual Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = _userRepository.GetUserById(userId);
            var hashed = EncryptUltis.Sha256Encrypt(oldPassword, user.SecurityStamp);
            if (hashed != user.PasswordHash) throw new InvalidDataException("Incorrect old password");
            var newPasswordHash = EncryptUltis.Sha256Encrypt(newPassword, user.SecurityStamp);
            user.PasswordHash = newPasswordHash;
            await _userRepository.UpdatePassword(user);
            return true;

        }
    }
}
