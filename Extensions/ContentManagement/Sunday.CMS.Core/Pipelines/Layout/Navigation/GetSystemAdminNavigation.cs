using Sunday.CMS.Core.Models.Layout;
using Sunday.CMS.Core.Pipelines.Arguments;
using System.Threading.Tasks;
using Sunday.Core.Constants;
using Sunday.Core.Pipelines;

namespace Sunday.CMS.Core.Pipelines.Layout.Navigation
{
    public class GetSystemAdminNavigation : IAsyncPipelineProcessor
    {
        public Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (GetNavigationArg) pipelineArg;
            if (!arg.User.IsInRole(SystemRoleCodes.SystemAdmin)) return Task.CompletedTask;
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
            arg.NavigationItems.Add(new NavigationItem()
            {
                Link = "/manage-layouts",
                Section = "Manage Contents",
                Title = "Manage Layouts",
                IconClass = "pe-7s-next-2"
            });
            arg.NavigationItems.Add(new NavigationItem()
            {
                Link = "/manage-templates",
                Section = "Manage Contents",
                Title = "Manage Templates",
                IconClass = "lnr-book"
            });
            arg.NavigationItems.Add(new NavigationItem()
            {
                Link = "/manage-renderings",
                Section = "Manage Contents",
                Title = "Manage Renderings",
                IconClass = "pe-7s-news-paper"
            });
            arg.NavigationItems.Add(new NavigationItem()
            {
                Link = "/manage-websites",
                Section = "Manage Contents",
                Title = "Manage Websites",
                IconClass = "pe-7s-ball"
            });
            arg.NavigationItems.Add(new NavigationItem()
            {
                Link = "/manage-contents",
                Section = "Manage Contents",
                Title = "Manage Contents",
                IconClass = "pe-7s-box2"
            });
            return Task.CompletedTask;
        }
    }
}
