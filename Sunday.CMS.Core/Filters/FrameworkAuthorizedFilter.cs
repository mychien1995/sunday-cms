using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Sunday.Core.Application.Identity;
using Sunday.Core.Identity;
using Sunday.Core.Models.Users;
using Sunday.Core.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

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
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (actionDescriptor == null) return;
            var skipAuthorization = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Length > 0;
            if (skipAuthorization) return;
            var authToken = context.HttpContext.Request.Headers.FirstOrDefault(x => x.Key.ToUpper() == "Authorization".ToUpper()).Value.ToString();
            if (string.IsNullOrEmpty(authToken))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            int? userId;
            if (!_accessTokenService.ValidToken(authToken, out userId) || userId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var user = _userRepository.GetUserWithOptions(userId.Value, new GetUserOptions()
            {
                FetchRoles = true
            });
            if (user == null || user.IsDeleted || !user.IsActive || user.IsLockedOut)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            context.HttpContext.User = new ApplicationUserPrincipal(user);
            return;
        }
    }
}
