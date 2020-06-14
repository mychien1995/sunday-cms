using Microsoft.Extensions.DependencyInjection;

namespace Sunday.Core.Pipelines.Arguments
{
    public class InitializationArg : PipelineArg
    {
        public IServiceCollection ServiceCollection { get; set; }
        public InitializationArg(IServiceCollection serviceCollection)
        {
            this.ServiceCollection = serviceCollection;
        }
    }
}
