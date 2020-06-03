using Microsoft.AspNetCore.Http;
using Sunday.CMS.Core.Application.Organizations;
using Sunday.Core;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

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
            throw new NotImplementedException();
        }
    }
}
