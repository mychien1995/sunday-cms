using Microsoft.Extensions.DependencyInjection;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.CMS.Core.Pipelines.ConfigureServices
{
    public class AddCorsPolicy : IPipelineProcessor
    {
        public void Process(PipelineArg arg)
        {
            var serviceCollection = ((ConfigureServicesArg)arg).ServicesCollection;
            serviceCollection.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
        }
    }
}
