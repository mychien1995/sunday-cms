using AutoMapper;
using Sunday.Core.Application.Identity;
using Sunday.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.Implementation.Identity
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

        public async Task<AuthenticationResponse> AuthenticateAsync(string username, string password, bool loginToShell = false, bool remember = false)
        {
            var signInResult = await _identityService.PasswordSignInAsync(username, password, loginToShell, remember);
            if (signInResult.Status != LoginStatus.Success || signInResult.User == null) return new AuthenticationResponse(signInResult.Status);
            var token = _accessTokenService.GenerateToken(signInResult.User);
            return new AuthenticationResponse(signInResult, token);
        }
    }
}
