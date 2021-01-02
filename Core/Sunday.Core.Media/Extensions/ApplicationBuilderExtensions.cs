using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Sunday.Core.Media.Application;

namespace Sunday.Core.Media.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseBlobPreview(this IApplicationBuilder app)
        {
            app.Map($"{RoutePaths.BlobPreviewRoute}", HandlePreviewLink);
        }
        public static void HandlePreviewLink(IApplicationBuilder app)
        {
            var blobProvider = app.ApplicationServices.GetService<IBlobProvider>();
            app.Run(async context =>
            {
                var path = context.Request.Path.Value;
                var identifier = path.Replace(RoutePaths.BlobPreviewRoute, "");
                var blob = blobProvider.GetBlob(identifier);
                if (blob != null)
                {
                    await using var stream = blob.OpenRead();
                    await using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    var bytes = ms.ToArray();
                    context.Response.ContentType = "image/jpeg";
                    context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=86400";
                    await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                }
            });
        }
    }
}
