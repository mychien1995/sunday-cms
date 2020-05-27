using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

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
