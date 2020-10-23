using Microsoft.AspNetCore.Builder;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.Core.Framework
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseSundayMiddleware(this IApplicationBuilder app)
        {
            ApplicationPipelines.RunAsync("buildApplication", new BuildApplicationArg(app)).Wait();
        }
    }
}
