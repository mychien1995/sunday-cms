using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Users;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpContextExtensions
    {
        public static void AddOrganization(this HttpContext context, IApplicationOrganization organization)
        {
            context.Items.TryAdd("current_organization", organization);
        }

        public static IApplicationOrganization GetOrganization(this HttpContext context)
        {
            context.Items.TryGetValue("current_organization", out object org);
            if (org == null) return null;
            return org as IApplicationOrganization;
        }

        public static IApplicationUser GetCurrentUser(this HttpContext context)
        {
            context.Items.TryGetValue("current_user", out object user);
            if (user == null) return null;
            return user as IApplicationUser;
        }

        public static void SetCurrentUser(this HttpContext context, IApplicationUser user)
        {
            context.Items.TryAdd("current_user", user);
        }
    }
}
