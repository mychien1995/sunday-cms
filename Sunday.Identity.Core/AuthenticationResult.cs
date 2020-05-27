using System;
using System.Collections.Generic;
using System.Text;
using Sunday.Core.Domain.Users;

namespace Sunday.Identity.Core
{
    public class AuthenticationResult<T> where T : IApplicationUser
    {
        public LoginStatus SignInResult { get; set; }
        public T User { get; set; }
        public string AccessToken { get; set; }
        public AuthenticationResult(LoginStatus signInResult)
        {
            this.SignInResult = signInResult;
        }
        public AuthenticationResult(SignInResult<T> signInResult, string token)
        {
            this.SignInResult = signInResult.Status;
            this.User = signInResult.User;
            this.AccessToken = token;
        }
    }
}
