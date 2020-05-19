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
                var hashed = EncryptUtils.SHA256Encrypt(password, user.SecurityStamp);
                if (hashed != user.PasswordHash) status = LoginStatus.Failure;
                else if (!user.EmailConfirmed) status = LoginStatus.RequiresVerification;
                else if (user.Domain != DomainNames.Shell && loginToShell) status = LoginStatus.Failure;
                else status = LoginStatus.Success;
            }
            if (status == LoginStatus.Success)
                return new SignInResult(status, user);
            return new SignInResult(status);
        }
    }
}
