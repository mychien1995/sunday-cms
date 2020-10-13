using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Sunday.CMS.Core.Application;
using Sunday.Foundation.Domain;

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
            if (!(context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)) return;
            var skipAuthorization = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Length > 0;
            if (skipAuthorization) return;
            var user = context.HttpContext.User as ApplicationUserPrincipal;
            if (!_accessManager.AllowAccess(user.User))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    message = "You are not allowed to access this hostname"
                });
            }
        }
    }
}
