using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Sunday.Foundation.Domain
{
    public class ApplicationUserPrincipal : ClaimsPrincipal, IApplicationUserPrincipal
    {
        public ApplicationUser User { get; set; }
        public ApplicationUserPrincipal(ApplicationUser user)
        {
            this.Identity = new GenericIdentity(user.Id.ToString());
            this.User = user;
            this.UserId = user.Id;
            this.Username = user.UserName;
            this.Fullname = user.Fullname;
            this.Email = user.Email;
            this.Roles = user.Roles;
        }
        public override IIdentity Identity { get; }

        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public List<ApplicationRole> Roles { get; set; }

        public override bool IsInRole(string roleCode) => User.IsInRole(roleCode);
    }
}
