using Microsoft.AspNetCore.Builder;

namespace Sunday.Core.Pipelines.Arguments
{
    public class InitializationArg : PipelineArg
    {
        public InitializationArg(IApplicationBuilder appBuilder)
        {
            AppBuilder = appBuilder;
        }

        public IApplicationBuilder AppBuilder { get; set; }
    }
}
