using System;
using System.Linq;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Extensions;
using Sunday.Core;
using Sunday.Core.Constants;
using Sunday.Core.Extensions;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Implementation
{
    [ServiceTypeOf(typeof(IOrganizationAccessManager))]
    public class DefaultOrganizationAccessManager : IOrganizationAccessManager
    {
        private readonly IOrganizationService _organizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DefaultOrganizationAccessManager(IOrganizationService organizationService, IHttpContextAccessor httpContextAccessor)
        {
            _organizationService = organizationService;
            _httpContextAccessor = httpContextAccessor;
        }
        public bool AllowAccess(ApplicationUser user)
        {
            if (user.IsInRole(SystemRoleCodes.SystemAdmin))
                return true;
            var hostName = GetHostName();
            if (string.IsNullOrEmpty(hostName))
                return true;
            var sharedHostNames = ApplicationSettings.Get<string>("Sunday.CMS.SharedHosts");
            if (!string.IsNullOrEmpty(sharedHostNames) && sharedHostNames.ToLower().Split('|').Contains(hostName))
                return true;
            var organization = ResolveOrganizationFromRequest();
            if (organization.IsNone) return false;
            if (user.OrganizationUsers.All(c => c.OrganizationId != organization.Get().Id)) return false;
            _httpContextAccessor.HttpContext.AddOrganization(organization.Get());
            return true;
        }

        public Option<ApplicationOrganization> ResolveOrganizationFromRequest()
        {
            if (_httpContextAccessor.HttpContext.User is ApplicationUserPrincipal user &&
                (user.IsInRole(SystemRoleCodes.OrganizationAdmin) || user.IsInRole(SystemRoleCodes.OrganizationUser)))
            {
                var organizationId = user.User.OrganizationUsers.FirstOrDefault()!.Organization!.Id;
                return _organizationService.GetOrganizationByIdAsync(organizationId).MapResultTo(o => o.Get())
                    .Result;
            }
            return Option<ApplicationOrganization>.None;
        }

        private string GetHostName()
        {
            var origin = _httpContextAccessor.HttpContext.Request.Headers.ContainsKey("origin") ?
                _httpContextAccessor.HttpContext.Request.Headers["origin"].FirstOrDefault()?.ToLower() : "";
            if (string.IsNullOrEmpty(origin))
                return string.Empty;
            var uri = new Uri(origin);
            return uri.Host;
        }
    }
}
