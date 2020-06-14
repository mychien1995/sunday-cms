﻿using Sunday.Core;
using Sunday.Core.Pipelines.Arguments;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseSundayMiddleware(this IApplicationBuilder app)
        {
            ApplicationPipelines.Run("buildApplication", new BuildApplicationArg(app));
        }
    }
}
