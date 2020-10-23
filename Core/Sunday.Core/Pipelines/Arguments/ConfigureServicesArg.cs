using Microsoft.Extensions.DependencyInjection;

namespace Sunday.Core.Pipelines.Arguments
{
    public class ConfigureServicesArg : PipelineArg
    {
        public IServiceCollection ServicesCollection { get; }

        public ConfigureServicesArg(IServiceCollection servicesCollection)
        {
            ServicesCollection = servicesCollection;
        }
    }
}
