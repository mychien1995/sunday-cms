using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Foundation
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
