using Sunday.Core.Domain.Identity;
using Sunday.Core.Domain.Roles;
using Sunday.Core.Domain.Users;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Sunday.Identity.Core
{
    public class ApplicationUserPrincipal : ClaimsPrincipal, IApplicationUserPrincipal
    {
        public IApplicationUser User { get; set; }
        public ApplicationUserPrincipal(IApplicationUser user)
        {
            this.Identity = new GenericIdentity(user.ID.ToString());
            this.User = user;
            this.UserId = user.ID;
            this.Username = user.UserName;
            this.Fullname = user.Fullname;
            this.Email = user.Email;
            this.Roles = user.Roles;
        }
        public override IIdentity Identity { get; }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public List<IApplicationRole> Roles { get; set; }

        public override bool IsInRole(string roleCode)
        {
            return User.IsInRole(roleCode);
        }
    }
}
