using System;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sunday.Core.Application;
using Sunday.Core.Configuration;
using Sunday.Core.Domain;
using Sunday.Core.Implementation;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.Core.Framework.Pipelines.ConfigureServices
{
    public class ResolveTcpRemoteEventHandler : IPipelineProcessor
    {
        private readonly ILogger<ResolveTcpRemoteEventHandler> _logger;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public ResolveTcpRemoteEventHandler(ILogger<ResolveTcpRemoteEventHandler> logger, ApplicationConfiguration applicationConfiguration)
        {
            _logger = logger;
            _applicationConfiguration = applicationConfiguration;
        }

        public void Process(PipelineArg pipelineArg)
        {
            var arg = (ConfigureServicesArg)pipelineArg;
            var configXml = _applicationConfiguration.ConfigurationXml;
            var remoteEventNode = configXml.SelectSingleNode("/configuration/remoteEvent");
            if (remoteEventNode == null)
            {
                _logger.LogInformation("No remove event handler found");
                return;
            }
            var type = remoteEventNode!.Attributes!["handler"].Value;
            if (!type.Equals("tcp", StringComparison.OrdinalIgnoreCase)) return;
            _logger.LogInformation("Remote Event Handler is TCP");
            var listeningAddress = string.Empty;
            var publishingAddress = string.Empty;
            foreach (XmlNode? child in remoteEventNode.ChildNodes)
            {
                if (child!.Name == "listening")
                {
                    listeningAddress = child.InnerText;
                }
                else if(child!.Name == "publishing")
                {
                    publishingAddress = child.InnerText;
                }
            }
            var configuration = new TcpRemoteEventConfiguration(listeningAddress, publishingAddress);
            arg.ServicesCollection.AddSingleton(new RemoteEventConfiguration("TCP"));
            arg.ServicesCollection.AddSingleton(configuration);
            var remoteEventHandler =
                (TcpRemoteEventHandler)ActivatorUtilities.CreateInstance(arg.ServicesCollection.BuildServiceProvider(), typeof(TcpRemoteEventHandler));
            remoteEventHandler.Initialize();
            arg.ServicesCollection.AddSingleton<IRemoteEventHandler>(remoteEventHandler);

        }
    }
}
