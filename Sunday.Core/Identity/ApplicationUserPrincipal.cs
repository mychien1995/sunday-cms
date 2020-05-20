using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Sunday.Core.Identity
{
    public class ApplicationUserPrincipal : ClaimsPrincipal
    {
        public ApplicationUserPrincipal(int userId)
        {
            this.Identity = new GenericIdentity(userId.ToString());
        }
        public ApplicationUserPrincipal(ApplicationUser user)
        {
            this.Identity = new GenericIdentity(user.ID.ToString());
            this.UserId = user.ID;
            this.Username = user.UserName;
            this.Fullname = user.Fullname;
            this.Email = user.Email;
        }
        public override IIdentity Identity { get; }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
    }
}
