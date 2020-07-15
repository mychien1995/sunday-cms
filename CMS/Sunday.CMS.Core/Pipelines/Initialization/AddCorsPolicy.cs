using Microsoft.Extensions.DependencyInjection;
using Sunday.Core;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.CMS.Core.Pipelines.Initialization
{
    public class AddCorsPolicy
    {
        public void Process(PipelineArg arg)
        {
            var serviceCollection = (arg as InitializationArg)?.ServiceCollection;
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
