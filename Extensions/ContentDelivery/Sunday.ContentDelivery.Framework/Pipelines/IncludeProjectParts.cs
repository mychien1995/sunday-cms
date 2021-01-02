using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using Sunday.ContentDelivery.Framework.Attributes;
using Sunday.Core;
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
            foreach (var projectAssembly in assemblies)
            {
                mvcBuilder
                    .AddApplicationPart(projectAssembly);
                providers.Add(new EmbeddedFileProvider(projectAssembly, projectAssembly.GetName().Name!));
            }
            mvcBuilder.AddRazorRuntimeCompilation(opt => providers.Iter(provider => opt.FileProviders.Add(provider)));
            var assetsPath = ApplicationSettings.Get<string>("Sunday.CD.AssetPath");
            if (string.IsNullOrEmpty(assetsPath))
                assetsPath = $"{Directory.GetCurrentDirectory()}\\wwwroot";
            services.Configure<StaticFileOptions>(opt =>
            {
                var paths = assetsPath.Split('|');
                var provider = new CompositeFileProvider(paths.Select(p => new PhysicalFileProvider(p)));
                opt.OnPrepareResponse = ctx => ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                    "public,max-age=86400";
                opt.FileProvider = provider;
            });
        }
    }
}