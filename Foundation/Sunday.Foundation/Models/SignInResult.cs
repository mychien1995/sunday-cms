using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Models
{
    public class SignInResult
    {
        private readonly AuthenticationResult _authenticationResult;
        public LoginStatus Result => _authenticationResult.Status;
        public ApplicationUser User => _authenticationResult.User;
        public string AccessToken { get; }
        public SignInResult(AuthenticationResult authenticationResult, string token)
        {
            AccessToken = token;
            this._authenticationResult = authenticationResult;
        }
    }
}
