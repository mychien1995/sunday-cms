using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Domain.Users
{
    public static class UserExtensions
    {
        public static bool IsInOneOfRoles(this IApplicationUser user, params string[] roles)
        {
            foreach(var role in roles)
            {
                if (user.IsInRole(role)) return true;
            }
            return false;
        }
    }
}
