using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sunday.ContentDelivery.Core;
using Sunday.ContentDelivery.Core.Services;
using Sunday.Core.Extensions;

namespace Sunday.ContentDelivery.Framework.Middlewares
{
    public class SiteContextMiddleware
    {
        private readonly IWebsiteLoader _websiteLoader;
        private readonly RequestDelegate _next;

        public SiteContextMiddleware(IWebsiteLoader websiteLoader, RequestDelegate next)
        {
            _websiteLoader = websiteLoader;
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            var hostName = httpContext.Request.Host.Value;
            var websiteOpt = await _websiteLoader.GetWebsiteByHostName(hostName);
            if (websiteOpt.IsNone) goto NEXT;
            var website = websiteOpt.Get();
            httpContext.Items.Add(RouteDataKey.Website, website);
            NEXT:
            await _next(httpContext);
        }
    }
}
