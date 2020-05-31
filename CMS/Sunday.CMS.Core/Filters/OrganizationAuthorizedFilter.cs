using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Sunday.CMS.Core.Application.Organizations;
using Sunday.Core;
using Sunday.Identity.Core;
using Sunday.Organizations.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Filters
{
    public class OrganizationAuthorizedFilter : IAuthorizationFilter
    {
        private readonly IOrganizationAccessManager _accessManager;
        public OrganizationAuthorizedFilter(IOrganizationAccessManager accessManager)
        {
            _accessManager = accessManager;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (actionDescriptor == null) return;
            var skipAuthorization = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Length > 0;
            if (skipAuthorization) return;
            var user = context.HttpContext.User as ApplicationUserPrincipal;
            if (!_accessManager.AllowAccess(user.User))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    message = "You are not allowed to access this hostname"
                });
                return;
            }
        }
    }
}
