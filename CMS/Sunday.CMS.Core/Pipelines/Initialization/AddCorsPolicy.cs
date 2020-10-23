using Microsoft.Extensions.DependencyInjection;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.CMS.Core.Pipelines.Initialization
{
    public class AddCorsPolicy
    {
        public void Process(ConfigureServicesArg arg)
        {
            var serviceCollection = arg.ServicesCollection;
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
