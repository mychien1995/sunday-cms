using System;
using Sunday.CMS.Core.Models.Layout;
using Sunday.CMS.Core.Pipelines.Arguments;
using System.Linq;
using System.Threading.Tasks;
using Sunday.Core.Constants;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Context;

namespace Sunday.CMS.Core.Pipelines.Layout.Navigation
{
    public class GetOrganizationAdminNavigation : IAsyncPipelineProcessor
    {
        private readonly ISundayContext _sundayContext;
        public GetOrganizationAdminNavigation(ISundayContext sundayContext)
        {
            _sundayContext = sundayContext;
        }
        public Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (GetNavigationArg)pipelineArg;
            if (!arg.User.IsInRole(SystemRoleCodes.OrganizationAdmin)) return Task.CompletedTask;
            var organization = _sundayContext.CurrentOrganization;
            if (organization == null) throw new InvalidOperationException("Current Organization is undefined");
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
            if (modules.All(c => c != SystemModules.RolesManagement.Code)) return Task.CompletedTask;
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
            return Task.CompletedTask;
        }
    }
}
