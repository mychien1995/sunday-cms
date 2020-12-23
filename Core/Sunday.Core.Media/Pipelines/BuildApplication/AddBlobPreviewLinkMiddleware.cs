using Microsoft.Extensions.DependencyInjection;
using Sunday.Core.Media.Application;
using Sunday.Core.Pipelines.Arguments;
using Sunday.Core.Media.Extensions;
using Sunday.Core.Media.Implementation;
using Sunday.Core.Pipelines;

namespace Sunday.Core.Media.Pipelines.BuildApplication
{
    public class AddBlobPreviewLinkMiddleware : IPipelineProcessor
    {
        public void Process(PipelineArg arg)
        {
            var app = ((InitializationArg)arg).AppBuilder;
            var blobProvider = app.ApplicationServices.GetService<IBlobProvider>();
            if (blobProvider is FileBlobProvider)
                app.UseBlobPreview();
        }
    }
}
