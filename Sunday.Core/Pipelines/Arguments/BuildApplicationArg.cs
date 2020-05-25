using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

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
