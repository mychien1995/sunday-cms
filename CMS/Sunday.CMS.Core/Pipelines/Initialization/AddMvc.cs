using Microsoft.Extensions.DependencyInjection;
using Sunday.Core;
using Sunday.Core.Pipelines.Arguments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Pipelines.Initialization
{
    public class AddMvc
    {
        public void Process(PipelineArg arg)
        {
            var serviceCollection = (arg as InitializationArg).ServiceCollection;
            serviceCollection.AddMvc()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
        }
    }
}
