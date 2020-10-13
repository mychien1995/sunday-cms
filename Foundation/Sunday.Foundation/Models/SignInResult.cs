using LanguageExt;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Models
{
    public class SignInResult
    {
        private readonly AuthenticationResult _authenticationResult;
        public LoginStatus Status => _authenticationResult.Status;
        public Option<ApplicationUser> User => _authenticationResult.User;
        public string AccessToken { get; } = string.Empty;
        public SignInResult(AuthenticationResult authenticationResult, string? token = null)
        {
            this._authenticationResult = authenticationResult;
            if (token != null)
                AccessToken = token;
        }
    }
}
