using Microsoft.AspNetCore.Http;
using Sunday.CMS.Core.Application.Organizations;
using Sunday.Core;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Users;
using Sunday.Identity.Core;
using Sunday.Organizations.Application;
using Sunday.Organizations.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Implementation.Organizations
{
    [ServiceTypeOf(typeof(IOrganizationAccessManager))]
    public class DefaultOrganizationAccessManager : IOrganizationAccessManager
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DefaultOrganizationAccessManager(IOrganizationRepository organizationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _organizationRepository = organizationRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public bool AllowAccess(IApplicationUser user)
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
            if (organization == null) return false;
            if (!user.OrganizationUsers.Any(c => c.OrganizationId == organization.ID)) return false;
            _httpContextAccessor.HttpContext.AddOrganization(organization);
            return true;
        }

        public IApplicationOrganization ResolveOrganizationFromRequest()
        {
            var hostName = GetHostName();
            if (string.IsNullOrEmpty(hostName))
                return null;
            var organization = Task.Run(async () => { return await _organizationRepository.FindOrganizationByHostname(hostName); }).Result;
            if (organization == null || organization.IsDeleted || !organization.IsActive) organization = null;
            if (organization == null)
            {
                var user = _httpContextAccessor.HttpContext.User as ApplicationUserPrincipal;
                if (user.IsInRole(SystemRoleCodes.OrganizationAdmin) || user.IsInRole(SystemRoleCodes.OrganizationUser))
                {
                    organization = user.User.OrganizationUsers.FirstOrDefault().Organization as ApplicationOrganization;
                }
            }
            return organization;
        }

        private string GetHostName()
        {
            var origin = _httpContextAccessor.HttpContext.Request.Headers.ContainsKey("origin") ?
                _httpContextAccessor.HttpContext.Request.Headers["origin"].FirstOrDefault().ToLower() : "";
            if (string.IsNullOrEmpty(origin))
                return string.Empty;
            var uri = new Uri(origin);
            return uri.Host;
        }
    }
}
