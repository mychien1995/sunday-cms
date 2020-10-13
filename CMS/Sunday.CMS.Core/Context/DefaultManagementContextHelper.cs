﻿using System;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Extensions;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Foundation.Domain;

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
        public ApplicationOrganization GetCurrentOrganization()
        {
            var organization = _httpContextAccessor.HttpContext.GetCurrentOrganization();
            if (!organization.IsNone) throw new InvalidOperationException("Current Organization Unidentified");
            organization = _organizationAccessManager.ResolveOrganizationFromRequest();
            _httpContextAccessor.HttpContext.AddOrganization(organization.Get());
            return organization.Get();
        }

        public ApplicationUser GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext.GetCurrentUser();
            if (!user.IsNone) throw new InvalidOperationException("Current User Unidentified");
            user = ((ApplicationUserPrincipal)_httpContextAccessor.HttpContext.User).User;
            _httpContextAccessor.HttpContext.SetCurrentUser(user.Get());
            return user.Get();
        }
    }
}
