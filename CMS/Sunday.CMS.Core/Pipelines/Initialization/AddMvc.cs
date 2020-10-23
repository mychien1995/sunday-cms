using Microsoft.Extensions.DependencyInjection;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.CMS.Core.Pipelines.Initialization
{
    public class AddMvc
    {
        public void Process(ConfigureServicesArg arg)
        {
            var serviceCollection = arg.ServicesCollection;
            serviceCollection.AddMvc()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
        }
    }
}
