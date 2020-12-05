using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Sunday.Core.Extensions;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Filters
{
    public class FrameworkAuthorizedFilter : IAuthorizationFilter
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IUserService _userService;
        public FrameworkAuthorizedFilter(IAccessTokenService accessTokenService, IUserService userService)
        {
            _accessTokenService = accessTokenService;
            _userService = userService;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)) return;
            var skipAuthorization = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Length > 0;
            if (skipAuthorization) return;
            var authToken = context.HttpContext.Request.Headers.FirstOrDefault(x => string.
                Equals(x.Key, "Authorization", StringComparison.CurrentCultureIgnoreCase)).Value.ToString();
            if (string.IsNullOrEmpty(authToken))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!_accessTokenService.ValidToken(authToken, out var userId) || userId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var user = _userService.GetUserByIdAsync(userId.Value).Result;
            if (user.IsNone || user.Get().IsDeleted || !user.Get().IsActive || user.Get().IsLockedOut)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            context.HttpContext.User = new ApplicationUserPrincipal(user.Get());
        }
    }
}
