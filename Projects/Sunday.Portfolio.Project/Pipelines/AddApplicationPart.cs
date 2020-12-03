using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.Portfolio.Project.Pipelines
{
    public class AddApplicationPart : IPipelineProcessor
    {
        public void Process(PipelineArg arg)
        {
            var pipelineArg = (ConfigureServicesArg) arg;
            var assembly = GetType().Assembly;
            pipelineArg.ServicesCollection.AddControllersWithViews()
                .AddApplicationPart(assembly)
                .AddRazorRuntimeCompilation(opt =>
                {
                    opt.FileProviders.Add(new EmbeddedFileProvider(assembly, "Sunday.Portfolio.Project"));
                });
        }
    }
}
