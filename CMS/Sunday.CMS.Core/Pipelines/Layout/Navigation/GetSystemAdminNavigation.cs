using Sunday.CMS.Core.Models.Layout;
using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Pipelines.Layout.Navigation
{
    public class GetSystemAdminNavigation
    {
        public async Task ProcessAsync(GetNavigationArg arg)
        {
            if (!arg.User.IsInRole(SystemRoleCodes.SystemAdmin)) return;
            arg.NavigationItems.Add(new NavigationItem()
            {
                Link = "/users",
                Section = "Manage Users",
                Title = "Manage Users",
                IconClass = "pe-7s-diamond"
            });
            arg.NavigationItems.Add(new NavigationItem()
            {
                Link = "/organizations",
                Section = "Manage Organizations",
                Title = "Manage Organizations",
                IconClass = "pe-7s-car"
            });
        }
    }
}
