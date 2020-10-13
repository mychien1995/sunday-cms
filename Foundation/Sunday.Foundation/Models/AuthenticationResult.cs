using LanguageExt;
using Sunday.Foundation.Domain;
using static LanguageExt.Prelude;

namespace Sunday.Foundation.Models
{
    public class AuthenticationResult
    {
        public LoginStatus Status { get; set; }
        public Option<ApplicationUser> User { get; set; }
        public Option<string> AccessToken { get; set; }
        public AuthenticationResult(LoginStatus status, ApplicationUser? user = null, string? token = null)
        {
            this.Status = status;
            this.User = Optional(user)!;
            this.AccessToken = Optional(token)!;
        }
    }
}
