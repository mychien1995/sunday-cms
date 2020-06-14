using Microsoft.AspNetCore.Http;
using Sunday.CMS.Core.Application.Layout;
using Sunday.CMS.Core.Models.Layout;
using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core;
using Sunday.Identity.Core;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Implementation.Layout
{
    [ServiceTypeOf(typeof(INavigationManager))]
    public class DefaultNavigationManager : INavigationManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DefaultNavigationManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<NavigationTreeResponse> GetUserNavigation()
        {
            var user = (_httpContextAccessor.HttpContext.User as ApplicationUserPrincipal).User;
            var arg = new GetNavigationArg(user);
            await ApplicationPipelines.RunAsync("cms.layout.getNavigation", arg);
            var navigationItems = arg.NavigationItems;
            var response = new NavigationTreeResponse();
            response.Sections = navigationItems.GroupBy(x => x.Section).Select(x => new NavigationTreeSection()
            {
                Section = x.Key,
                Items = x.ToList()
            }).ToList();
            return response;
        }
    }
}
