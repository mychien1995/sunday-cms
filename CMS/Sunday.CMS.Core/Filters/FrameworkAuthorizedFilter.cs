using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Sunday.Identity.Application;
using Sunday.Identity.Core;
using Sunday.Users.Application;
using System.Linq;

namespace Sunday.CMS.Core.Filters
{
    public class FrameworkAuthorizedFilter : IAuthorizationFilter
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IUserRepository _userRepository;
        public FrameworkAuthorizedFilter(IAccessTokenService accessTokenService, IUserRepository userRepository)
        {
            _accessTokenService = accessTokenService;
            _userRepository = userRepository;
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
            var user = _userRepository.GetUserById(userId.Value);
            if (user == null || user.IsDeleted || !user.IsActive || user.IsLockedOut)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            context.HttpContext.User = new ApplicationUserPrincipal(user);
        }
    }
}
