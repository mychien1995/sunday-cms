using Sunday.Core;
using Sunday.Core.Domain.Users;
using Sunday.Identity.Application;
using Sunday.Identity.Core;
using System.Threading.Tasks;

namespace Sunday.Identity.Implementation
{
    [ServiceTypeOf(typeof(IAuthenticationService))]
    public class DefaultAuthenticationService : IAuthenticationService
    {
        private readonly IIdentityService _identityService;
        private readonly IAccessTokenService _accessTokenService;
        public DefaultAuthenticationService(IIdentityService identityService, IAccessTokenService accessTokenService)
        {
            _identityService = identityService;
            _accessTokenService = accessTokenService;
        }

        public async virtual Task<AuthenticationResult<IApplicationUser>> AuthenticateAsync(string username, string password, bool loginToShell = false, bool remember = false)
        {
            var signInResult = await _identityService.PasswordSignInAsync<IApplicationUser>(username, password, loginToShell, remember);
            if (signInResult.Status != LoginStatus.Success || signInResult.User == null) return new AuthenticationResult<IApplicationUser>(signInResult.Status);
            var token = _accessTokenService.GenerateToken(signInResult.User);
            return new AuthenticationResult<IApplicationUser>(signInResult, token);
        }
    }
}
