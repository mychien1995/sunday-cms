using Microsoft.AspNetCore.Http;
using Sunday.CMS.Core.Application.Organizations;
using Sunday.Core;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Users;
using Sunday.Identity.Core;

namespace Sunday.CMS.Core.Context
{
    [ServiceTypeOf(typeof(IManagementContextHelper))]
    public class DefaultManagementContextHelper : IManagementContextHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrganizationAccessManager _organizationAccessManager;
        public DefaultManagementContextHelper(IHttpContextAccessor httpContextAccessor, IOrganizationAccessManager organizationManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _organizationAccessManager = organizationManager;
        }
        public IApplicationOrganization GetCurrentOrganization()
        {
            var organization = _httpContextAccessor.HttpContext.GetOrganization();
            if (organization == null)
            {
                organization = _organizationAccessManager.ResolveOrganizationFromRequest();
                _httpContextAccessor.HttpContext.AddOrganization(organization);
            }
            return organization;
        }

        public IApplicationUser GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext.GetCurrentUser();
            if (user == null)
            {
                user = (_httpContextAccessor.HttpContext.User as ApplicationUserPrincipal).User;
                _httpContextAccessor.HttpContext.SetCurrentUser(user);
            }
            return user;
        }
    }
}
