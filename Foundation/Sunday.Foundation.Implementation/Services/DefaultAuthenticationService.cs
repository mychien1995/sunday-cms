using System.Threading.Tasks;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Models;

namespace Sunday.Foundation.Implementation.Services
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

        public async Task<SignInResult> PasswordSignInAsync(string username, string password, bool loginToShell = false, bool remember = false)
        {
            var authenticateResult = await _identityService.AuthenticateAsync(username, password, loginToShell, remember);
            if (authenticateResult.Status != LoginStatus.Success) return new SignInResult(authenticateResult);
            var token = _accessTokenService.GenerateToken(authenticateResult.User.Get());
            return new SignInResult(authenticateResult, token);
        }
    }
}
