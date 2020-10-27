using Microsoft.AspNetCore.Http;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        public static string CurrentUserName(this IHttpContextAccessor httpContextAccessor)
            => ((httpContextAccessor.HttpContext.User as IApplicationUserPrincipal)!).Username;
    }
}
