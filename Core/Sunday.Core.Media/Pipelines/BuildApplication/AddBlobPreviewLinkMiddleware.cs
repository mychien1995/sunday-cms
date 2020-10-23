using Microsoft.AspNetCore.Builder;
using Sunday.Core.Media.Application;
using Sunday.Core.Pipelines.Arguments;
using System.IO;
using Sunday.Core.Pipelines;

namespace Sunday.Core.Media.Pipelines.BuildApplication
{
    public class AddBlobPreviewLinkMiddleware : IPipelineProcessor
    {
        private readonly IBlobProvider _blobProvider;
        public AddBlobPreviewLinkMiddleware(IBlobProvider blobProvider)
        {
            _blobProvider = blobProvider;
        }
        public void Process(PipelineArg arg)
        {
            var app = ((BuildApplicationArg)arg).AppBuilder;
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
                    await using var stream = blob.OpenRead();
                    await using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    var bytes = ms.ToArray();
                    context.Response.ContentType = "image/jpeg";
                    await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                }
            });
        }
    }
}
