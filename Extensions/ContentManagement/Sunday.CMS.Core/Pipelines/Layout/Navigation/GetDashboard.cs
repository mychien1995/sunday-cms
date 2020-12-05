using Sunday.CMS.Core.Models.Layout;
using Sunday.CMS.Core.Pipelines.Arguments;
using System.Threading.Tasks;
using Sunday.Core.Pipelines;

namespace Sunday.CMS.Core.Pipelines.Layout.Navigation
{
    public class GetDashboard : IAsyncPipelineProcessor
    {
        public Task ProcessAsync(PipelineArg arg)
        {
            ((GetNavigationArg)arg).NavigationItems.Add(new NavigationItem()
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
