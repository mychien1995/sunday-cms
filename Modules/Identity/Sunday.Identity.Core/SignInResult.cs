
using Sunday.Core.Domain.Users;

namespace Sunday.Identity.Core
{
    public class SignInResult<T> where T : IApplicationUser
    {
        public LoginStatus Status { get; set; }
        public T User { get; set; }
        public SignInResult(LoginStatus status, IApplicationUser user = null)
        {
            this.Status = status;
            this.User = (T)user;
        }
    }
}
