using System;
using System.IO;
using System.Threading.Tasks;
using Sunday.Core;
using Sunday.Core.Constants;
using Sunday.Core.Extensions;
using Sunday.Core.Ultilities;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Models;
using Sunday.Foundation.Persistence.Application.Repositories;

namespace Sunday.Foundation.Implementation.Services
{
    [ServiceTypeOf(typeof(IIdentityService))]
    public class DefaultIdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;

        public DefaultIdentityService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(string username, string password, bool loginToShell = false, bool remember = false)
        {
            LoginStatus status;
            var userOpt = await _userRepository.FindUserByNameAsync(username);
            if (userOpt.IsNone) status = LoginStatus.Failure;
            else
            {
                var user = userOpt.Get();
                if (user.IsDeleted) status = LoginStatus.Failure;
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
                    return new AuthenticationResult(status, user.MapTo<ApplicationUser>());
            }
            return new AuthenticationResult(status);
        }

        public async Task<string> ResetPasswordAsync(Guid userId)
        {
            var userOpt = await _userRepository.GetUserByIdAsync(userId);
            if (userOpt.IsNone) throw new ArgumentException($"User {userId} not found");
            var user = userOpt.Get();
            var securityHash = Guid.NewGuid().ToString("N");
            var newPassword = PasswordUltils.RandomString(6);
            var passwordHash = EncryptUltis.Sha256Encrypt(newPassword, user.SecurityStamp);
            user.SecurityStamp = securityHash;
            user.PasswordHash = passwordHash;
            await _userRepository.UpdatePasswordAsync(user);
            return newPassword;
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
        {
            var userOpt = await _userRepository.GetUserByIdAsync(userId);
            if (userOpt.IsNone) throw new ArgumentException($"User {userId} not found");
            var user = userOpt.Get();
            var hashed = EncryptUltis.Sha256Encrypt(oldPassword, user.SecurityStamp);
            if (hashed != user.PasswordHash) throw new InvalidDataException("Incorrect old password");
            var newPasswordHash = EncryptUltis.Sha256Encrypt(newPassword, user.SecurityStamp);
            user.PasswordHash = newPasswordHash;
            await _userRepository.UpdatePasswordAsync(user);
            return true;
        }
    }
}
