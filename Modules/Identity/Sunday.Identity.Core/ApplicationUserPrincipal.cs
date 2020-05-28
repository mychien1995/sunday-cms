using Sunday.Core.Domain.Identity;
using Sunday.Core.Domain.Roles;
using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Sunday.Identity.Core
{
    public class ApplicationUserPrincipal : ClaimsPrincipal, IApplicationUserPrincipal
    {
        public ApplicationUserPrincipal(int userId)
        {
            this.Identity = new GenericIdentity(userId.ToString());
        }
        public ApplicationUserPrincipal(IApplicationUser user)
        {
            this.Identity = new GenericIdentity(user.ID.ToString());
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
            if (Roles == null || !Roles.Any()) return false;
            return Roles.Any(c => c.Code.ToLower() == roleCode.ToLower());
        }
    }
}
