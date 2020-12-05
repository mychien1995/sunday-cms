using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Layout;
using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Media.Application;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Context;
using Sunday.Foundation.Domain;
using static LanguageExt.Prelude;

namespace Sunday.CMS.Core.Implementation
{
    [ServiceTypeOf(typeof(ILayoutManager))]
    public class DefaultLayoutManager : ILayoutManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBlobLinkManager _blobLinkManager;
        private readonly ISundayContext _sundayContext;

        public DefaultLayoutManager(IHttpContextAccessor httpContextAccessor, IBlobLinkManager blobLinkManager, ISundayContext sundayContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _blobLinkManager = blobLinkManager;
            _sundayContext = sundayContext;
        }
        public async Task<NavigationTreeResponse> GetUserNavigation()
        {
            var user = ((ApplicationUserPrincipal)_httpContextAccessor.HttpContext.User).User;
            var arg = new GetNavigationArg(user);
            await ApplicationPipelines.RunAsync("cms.layout.getNavigation", arg);
            var navigationItems = arg.NavigationItems;
            var response = new NavigationTreeResponse
            {
                Sections = navigationItems.GroupBy(x => x.Section).Select(x => new NavigationTreeSection()
                {
                    Section = x.Key,
                    Items = x.ToList()
                }).ToList()
            };
            return response;
        }

        public GetLayoutResponse GetLayout()
        {
            var organization = Optional(_sundayContext.CurrentOrganization);
            return organization.Map(org => new GetLayoutResponse
            {
                Color = org!.Properties.Get("color").Map(c => c!.ToString()).IfNone(string.Empty)!,
                OrganizationName = org.OrganizationName,
                LogoUri = !string.IsNullOrEmpty(org.LogoBlobUri)
                    ? _blobLinkManager.GetPreviewLink(org.LogoBlobUri)
                    : string.Empty
            }).IfNone(GetLayoutResponse.Empty);
        }
    }
}
