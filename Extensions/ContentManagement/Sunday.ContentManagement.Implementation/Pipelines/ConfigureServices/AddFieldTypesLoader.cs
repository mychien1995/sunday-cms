using Microsoft.Extensions.DependencyInjection;
using Sunday.ContentManagement.Implementation.Services;
using Sunday.ContentManagement.Services;
using Sunday.Core.Configuration;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.ContentManagement.Implementation.Pipelines.ConfigureServices
{
    public class AddFieldTypesLoader : IPipelineProcessor
    {
        public void Process(PipelineArg pipelineArg)
        {
            var arg = (ConfigureServicesArg) pipelineArg;
            arg.ServicesCollection.AddSingleton<IFieldTypesProvider>(sp =>
            {
                var appConfig = sp.GetService<ApplicationConfiguration>();
                var fieldTypeLoader = new DefaultFieldTypesLoader();
                fieldTypeLoader.Initialize(appConfig);
                return fieldTypeLoader;
            });
        }
    }
}
