using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Sunday.MediaServer.Models;

namespace Sunday.MediaServer.Filters
{
    public class ApiKeyAuthorizedFilter : IAuthorizationFilter
    {
        private readonly ApiKeyConfiguration _configuration;

        public ApiKeyAuthorizedFilter(IOptionsMonitor<ApiKeyConfiguration> configuration)
        {
            _configuration = configuration.CurrentValue;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)) return;
            var skipAuthorization = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Length > 0;
            if (skipAuthorization) return;
            var apiKey = context.HttpContext.Request.Headers.FirstOrDefault(x => string.
                Equals(x.Key, "x-api-key", StringComparison.CurrentCultureIgnoreCase)).Value.ToString();
            if (string.IsNullOrEmpty(apiKey) || !_configuration.ApiKeys.Contains(apiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
