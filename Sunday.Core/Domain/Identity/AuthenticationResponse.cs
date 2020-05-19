using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Domain.Identity
{
    public class AuthenticationResponse
    {
        public LoginStatus SignInResult { get; set; }
        public ApplicationUser User { get; set; }
        public string AccessToken { get; set; }
        public AuthenticationResponse(LoginStatus signInResult)
        {
            this.SignInResult = signInResult;
        }
        public AuthenticationResponse(SignInResult signInResult, string token)
        {
            this.SignInResult = signInResult.Status;
            this.User = signInResult.User;
            this.AccessToken = token;
        }
    }
}
