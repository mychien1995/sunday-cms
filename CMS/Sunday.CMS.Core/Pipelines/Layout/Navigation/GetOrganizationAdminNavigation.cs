﻿using Sunday.CMS.Core.Models.Layout;
using Sunday.CMS.Core.Pipelines.Arguments;
using System.Linq;
using System.Threading.Tasks;
using Sunday.Core.Constants;
using Sunday.Foundation.Context;

namespace Sunday.CMS.Core.Pipelines.Layout.Navigation
{
    public class GetOrganizationAdminNavigation
    {
        private readonly ISundayContext _sundayContext;
        public GetOrganizationAdminNavigation(ISundayContext sundayContext)
        {
            _sundayContext = sundayContext;
        }
        public async Task ProcessAsync(GetNavigationArg arg)
        {
            if (!arg.User.IsInRole(SystemRoleCodes.OrganizationAdmin)) return;
            var organization = _sundayContext.CurrentOrganization;
            var modules = organization.Modules.Select(x => x.ModuleCode).ToList();
            if (modules.Any(c => c == SystemModules.UsersManagement.Code))
            {
                arg.NavigationItems.Add(new NavigationItem()
                {
                    Link = "/organization-users",
                    Section = "Manage Users",
                    Title = "Manage Users",
                    IconClass = "pe-7s-diamond"
                });
            }
            if (modules.Any(c => c == SystemModules.OrganizationProfileManagement.Code))
            {
                arg.NavigationItems.Add(new NavigationItem()
                {
                    Link = "/organization-profile",
                    Section = "Organization Profile",
                    Title = "Organization Profile",
                    IconClass = "pe-7s-car"
                });
            }
            if (modules.Any(c => c == SystemModules.RolesManagement.Code))
            {
                arg.NavigationItems.Add(new NavigationItem()
                {
                    Link = "/organization-roles",
                    Section = "Manage Roles",
                    Title = "Manage Roles",
                    IconClass = "pe-7s-browser"
                });
                arg.NavigationItems.Add(new NavigationItem()
                {
                    Link = "/permissions-manager",
                    Section = "Manage Roles",
                    Title = "Permissions Manager",
                    IconClass = "pe-7s-display2"
                });
            }
        }
    }
}
