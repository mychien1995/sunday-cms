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
            context.Items.TryGetValue("current_organization", out var org);
            return org as IApplicationOrganization;
        }

        public static IApplicationUser GetCurrentUser(this HttpContext context)
        {
            context.Items.TryGetValue("current_user", out var user);
            return user as IApplicationUser;
        }

        public static void SetCurrentUser(this HttpContext context, IApplicationUser user)
        {
            context.Items.TryAdd("current_user", user);
        }
    }
}
