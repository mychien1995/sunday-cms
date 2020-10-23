using Microsoft.Extensions.DependencyInjection;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.CMS.Core.Pipelines.ConfigureServices
{
    public class AddMvc : IPipelineProcessor
    {
        public void Process(PipelineArg arg)
        {
            var serviceCollection = ((ConfigureServicesArg)arg).ServicesCollection;
            serviceCollection.AddMvc()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
        }
    }
}
