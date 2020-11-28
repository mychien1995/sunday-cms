using Microsoft.AspNetCore.Http;
using Sunday.ContentDelivery.Core;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Models;
using Sunday.Core.Extensions;

namespace Sunday.ContentDelivery.Framework.Extensions
{
    public static class HttpContextExtensions
    {
        public static string? CurrentLayout(this HttpContext context)
        {
            var layoutOpt = context.Items.Get(RouteDataKey.Layout);
            return layoutOpt.IsSome ? (string?)layoutOpt.Get() : null;
        }
        public static ApplicationWebsite? CurrentWebsite(this HttpContext context)
        {
            var websiteOpt = context.Items.Get(RouteDataKey.Website);
            return websiteOpt.IsSome ? (ApplicationWebsite?)websiteOpt.Get() : null;
        }
        public static Content? CurrentPage(this HttpContext context)
        {
            var contentOpt = context.Items.Get(RouteDataKey.Content);
            return contentOpt.IsSome ? (Content?)contentOpt.Get() : null;
        }
    }
}
