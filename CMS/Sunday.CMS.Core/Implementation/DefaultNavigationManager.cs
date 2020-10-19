using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Layout;
using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Implementation
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
            var user = ((ApplicationUserPrincipal) _httpContextAccessor.HttpContext.User).User;
            var arg = new GetNavigationArg(user);
            await ApplicationPipelines.RunAsync("cms.layout.getNavigation", arg);
            var navigationItems = arg.NavigationItems;
            var response = new NavigationTreeResponse
            {
                Sections = navigationItems.GroupBy(x => x.Section).Select(x => new NavigationTreeSection()
                {
                    Section = x.Key, Items = x.ToList()
                }).ToList()
            };
            return response;
        }
    }
}
