using Microsoft.AspNetCore.Http;
using Sunday.Core.Domain.Identity;

namespace Sunday.Core.Framework.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        public static string CurrentUserName(this IHttpContextAccessor httpContextAccessor)
            => ((httpContextAccessor.HttpContext.User as IApplicationUserPrincipal)!).Username;
    }
}
