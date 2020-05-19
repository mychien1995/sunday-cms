using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Domain.Identity
{
    public class SignInResult
    {
        public LoginStatus Status { get; set; }
        public ApplicationUser User { get; set; }
        public SignInResult(LoginStatus status, ApplicationUser user = null)
        {
            this.Status = status;
            this.User = user;
        }
    }
    public enum LoginStatus
    {
        Success,
        LockedOut,
        RequiresVerification,
        Failure,
        OrganizationInActive
    }
}
