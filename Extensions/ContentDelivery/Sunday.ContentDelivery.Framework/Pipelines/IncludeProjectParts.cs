using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Sunday.ContentDelivery.Framework.Attributes;
using Sunday.Core.Framework.Helpers;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.ContentDelivery.Framework.Pipelines
{
    public class IncludeProjectParts : IPipelineProcessor
    {
        public void Process(PipelineArg arg)
        {
            var assemblies = AssemblyHelper.GetAssembliesWithAttribute(typeof(SundayProjectAttribute));
            var pipelineArg = (ConfigureServicesArg)arg;
            var services = pipelineArg.ServicesCollection;
            var mvcBuilder = services.AddControllersWithViews();
            var providers = new List<IFileProvider>();
            var fileProviders = new List<IFileProvider>();
            foreach (var projectAssembly in assemblies)
            {
                mvcBuilder
                    .AddApplicationPart(projectAssembly);
                providers.Add(new EmbeddedFileProvider(projectAssembly, projectAssembly.GetName().Name!));
                fileProviders.Add(new EmbeddedFileProvider(projectAssembly, $"{projectAssembly.GetName().Name!}.wwwroot"));
            }
            mvcBuilder.AddRazorRuntimeCompilation(opt => providers.Iter(provider => opt.FileProviders.Add(provider)));
            services.Configure<StaticFileOptions>(opt =>
            {
                var compositeProvider = new CompositeFileProvider(fileProviders);
                opt.FileProvider = compositeProvider;
            });
        }
    }
}