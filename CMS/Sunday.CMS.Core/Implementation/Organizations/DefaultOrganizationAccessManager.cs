using Microsoft.AspNetCore.Http;
using Sunday.CMS.Core.Application.Organizations;
using Sunday.Core;
using Sunday.Core.Domain.Users;
using Sunday.Identity.Core;
using Sunday.Organizations.Application;
using Sunday.Users.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var origin = _httpContextAccessor.HttpContext.Request.Headers.ContainsKey("origin") ?
                _httpContextAccessor.HttpContext.Request.Headers["origin"].FirstOrDefault().ToLower() : "";
            if (string.IsNullOrEmpty(origin))
                return true;
            var uri = new Uri(origin);
            var hostName = uri.Host;
            var sharedHostNames = ApplicationSettings.Get<string>("Sunday.CMS.SharedHosts");
            if (!string.IsNullOrEmpty(sharedHostNames) && sharedHostNames.ToLower().Split('|').Contains(hostName))
                return true;
            var organization = Task.Run(async () => { return await _organizationRepository.FindOrganizationByHostname(hostName); }).Result;
            if (organization == null || organization.IsDeleted || !organization.IsActive) return false;
            return true;
        }
    }
}
