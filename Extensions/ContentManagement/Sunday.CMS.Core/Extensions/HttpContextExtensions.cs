using System.Collections.Generic;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Sunday.Foundation.Domain;
using static LanguageExt.Prelude;

namespace Sunday.CMS.Core.Extensions
{
    public static class HttpContextExtensions
    {
        public static void AddOrganization(this HttpContext context, ApplicationOrganization organization)
        {
            context.Items.TryAdd("current_organization", organization);
        }

        public static Option<ApplicationOrganization> GetCurrentOrganization(this HttpContext context)
        {
            context.Items.TryGetValue("current_organization", out var org);
            return Optional(org).Map(o => (ApplicationOrganization)o!);
        }

        public static Option<ApplicationUser> GetCurrentUser(this HttpContext context)
        {
            context.Items.TryGetValue("current_user", out var user);
            return Optional(user).Map(o => (ApplicationUser)o!);
        }

        public static void SetCurrentUser(this HttpContext context, ApplicationUser user)
        {
            context.Items.TryAdd("current_user", user);
        }
    }
}
