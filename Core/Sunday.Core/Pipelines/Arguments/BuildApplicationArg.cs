using Microsoft.AspNetCore.Builder;

namespace Sunday.Core.Pipelines.Arguments
{
    public class BuildApplicationArg : PipelineArg
    {
        public IApplicationBuilder AppBuilder { get; set; }
        public BuildApplicationArg(IApplicationBuilder appBuilder)
        {
            this.AppBuilder = appBuilder;
        }
    }
}
