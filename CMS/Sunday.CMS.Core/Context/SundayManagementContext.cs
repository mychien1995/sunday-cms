using Sunday.Core;
using System;
using Microsoft.AspNetCore.Http;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Extensions;
using Sunday.Core.Extensions;
using Sunday.Foundation.Context;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Context
{
    [ServiceTypeOf(typeof(ISundayContext), LifetimeScope.PerRequest)]
    public class SundayManagementContext : ISundayContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrganizationAccessManager _organizationAccessManager;
        private readonly Lazy<ApplicationOrganization?> _currentOrganization;
        private readonly Lazy<ApplicationUser?> _currentUser;

        public SundayManagementContext(IOrganizationAccessManager organizationAccessManager, IHttpContextAccessor httpContextAccessor)
        {
            _organizationAccessManager = organizationAccessManager;
            _httpContextAccessor = httpContextAccessor;
            _currentOrganization = new Lazy<ApplicationOrganization?>(GetCurrentOrganization, true);
            _currentUser = new Lazy<ApplicationUser?>(GetCurrentUser, true);
        }

        private ApplicationOrganization? GetCurrentOrganization()
        {
            var organization = _httpContextAccessor.HttpContext.GetCurrentOrganization();
            if (organization.IsNone) return null;
            organization = _organizationAccessManager.ResolveOrganizationFromRequest();
            if (organization.IsNone) return null;
            _httpContextAccessor.HttpContext.AddOrganization(organization.Get());
            return organization.Get();
        }

        private ApplicationUser? GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext.GetCurrentUser();
            if (user.IsNone) return null;
            user = ((ApplicationUserPrincipal)_httpContextAccessor.HttpContext.User).User;
            if (user.IsNone) return null;
            _httpContextAccessor.HttpContext.SetCurrentUser(user.Get());
            return user.Get();
        }

        public ApplicationOrganization? CurrentOrganization => _currentOrganization.Value;

        public ApplicationUser? CurrentUser => _currentUser.Value;
    }
}
