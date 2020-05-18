using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.DataAccess.Pipelines.Initialization
{
    public class EnsureDbCreation
    {
        private readonly IConfiguration _configuration;
        public EnsureDbCreation(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Process(PipelineArg arg)
        {
            var test = _configuration.GetValue<string>("Environment");
        }
    }
}
