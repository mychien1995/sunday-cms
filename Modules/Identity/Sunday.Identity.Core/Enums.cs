using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Identity.Core
{
    public enum LoginStatus
    {
        Success,
        LockedOut,
        RequiresVerification,
        Failure,
        OrganizationInActive
    }
}
