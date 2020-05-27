using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Sunday.Core.Media.Application;
using Sunday.Core.Pipelines.Arguments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sunday.Core.Media.Pipelines.BuildApplication
{
    public class AddBlobPreviewLinkMiddleware
    {
        private readonly IBlobProvider _blobProvider;
        public AddBlobPreviewLinkMiddleware(IBlobProvider blobProvider)
        {
            _blobProvider = blobProvider;
        }
        public void Process(BuildApplicationArg arg)
        {
            var app = arg.AppBuilder;
            app.Map($"{RoutePaths.BlobPreviewRoute}", HandlePreviewLink);
        }

        public void HandlePreviewLink(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var identifier = context.Request.Path.Value.Replace(RoutePaths.BlobPreviewRoute, "");
                var blob = this._blobProvider.GetBlob(identifier);
                if (blob != null)
                {
                    using (var stream = blob.OpenRead())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            var bytes = ms.ToArray();
                            string base64String = Convert.ToBase64String(bytes);
                            context.Response.ContentType = "image/jpeg";
                            await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                        }
                    }
                }
            });
        }
    }
}
