using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core
{
    public enum SystemRoles
    {
        SystemAdmin = 1,
        Developer = 2,
        OrganizationAdmin = 3,
        OrganizationUser = 4
    }

    public class SystemRoleCodes
    {
        public const string SystemAdmin = "SA";
        public const string Developer = "DEV";
        public const string OrganizationAdmin = "AD";
        public const string OrganizationUser = "NOM";
    }
}
