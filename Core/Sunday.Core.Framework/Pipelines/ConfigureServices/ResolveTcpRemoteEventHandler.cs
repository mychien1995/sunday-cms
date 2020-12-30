using System;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sunday.Core.Application;
using Sunday.Core.Configuration;
using Sunday.Core.Implementation;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.Core.Framework.Pipelines.ConfigureServices
{
    public class ResolveTcpRemoteEventHandler : IPipelineProcessor
    {
        private static IRemoteEventHandler? _remoteEventHandler;
        private static TcpRemoteEventConfiguration? _remoteEventConfiguration;
        public void Process(PipelineArg pipelineArg)
        {
            var arg = (ConfigureServicesArg)pipelineArg;
            arg.ServicesCollection.AddSingleton(sp =>
            {
                if (_remoteEventConfiguration != null) return _remoteEventConfiguration;
                var logger = sp.GetService<ILogger<ResolveTcpRemoteEventHandler>>();
                var applicationConfiguration = sp.GetService<ApplicationConfiguration>();
                var configXml = applicationConfiguration.ConfigurationXml;
                var remoteEventNode = configXml.SelectSingleNode("/configuration/remoteEvent");
                if (remoteEventNode == null)
                {
                    logger.LogInformation("No remove event handler found");
                    return null;
                }
                var type = remoteEventNode!.Attributes!["handler"].Value;
                if (!type.Equals("tcp", StringComparison.OrdinalIgnoreCase)) return null!;
                logger.LogInformation("Remote Event Handler is TCP");
                var listeningAddress = string.Empty;
                var publishingAddress = string.Empty;
                foreach (XmlNode? child in remoteEventNode.ChildNodes)
                {
                    switch (child!.Name)
                    {
                        case "listening":
                            listeningAddress = child.InnerText;
                            break;
                        case "publishing":
                            publishingAddress = child.InnerText;
                            break;
                    }
                }
                _remoteEventConfiguration = new TcpRemoteEventConfiguration(listeningAddress, publishingAddress);
                return _remoteEventConfiguration;
            });
            arg.ServicesCollection.AddSingleton(sp =>
            {
                if (_remoteEventHandler != null) return _remoteEventHandler;
                var configuration = sp.GetService<TcpRemoteEventConfiguration>();
                if (configuration == null)
                {
                    _remoteEventHandler = new DummyRemoteEventHandler();
                    return _remoteEventHandler;
                }
                var remoteEventHandler =
                    (TcpRemoteEventHandler)ActivatorUtilities.CreateInstance(sp, typeof(TcpRemoteEventHandler));
                remoteEventHandler.Initialize();
                _remoteEventHandler = remoteEventHandler;
                return _remoteEventHandler;
            });
        }
    }
}
