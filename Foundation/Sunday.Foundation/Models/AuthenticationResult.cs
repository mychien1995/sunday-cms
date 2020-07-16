
using Sunday.Core.Domain.Users;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Models
{
    public class AuthenticationResult
    {
        public LoginStatus Status { get; set; }
        public ApplicationUser User { get; set; }
        public AuthenticationResult(LoginStatus status, ApplicationUser user = null)
        {
            this.Status = status;
            this.User = user;
        }
    }
}
