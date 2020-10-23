using Sunday.CMS.Core.Models.Layout;
using Sunday.CMS.Core.Pipelines.Arguments;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Pipelines.Layout.Navigation
{
    public class GetDashboard
    {
        public Task ProcessAsync(GetNavigationArg arg)
        {
            arg.NavigationItems.Add(new NavigationItem()
            {
                Link = "/",
                Section = "Dashboard",
                Title = "Dashboard",
                IconClass = "pe-7s-rocket"
            });
            return Task.CompletedTask;
        }
    }
}
