using Microsoft.AspNetCore.Http;
using Sunday.Core.Domain.Organizations;
using Sunday.Organizations.Core;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
