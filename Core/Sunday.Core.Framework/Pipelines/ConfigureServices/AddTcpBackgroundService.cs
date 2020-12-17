using Microsoft.Extensions.DependencyInjection;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.Core.Framework.Pipelines.ConfigureServices
{
    public class AddTcpBackgroundService : IPipelineProcessor
    {
        public void Process(PipelineArg pipelineArg)
        {
            var arg = (ConfigureServicesArg)pipelineArg;
            arg.ServicesCollection.AddHostedService<TcpEventListener>();
        }
    }
}
