using Microsoft.Extensions.DependencyInjection;
using Sunday.Core.Configuration;
using Sunday.Core.Media.Application;
using Sunday.Core.Pipelines.Arguments;
using System;
using System.Collections.Generic;
using System.Xml;
using Sunday.Core.Pipelines;

namespace Sunday.Core.Media.Pipelines.ConfigureServices
{
    public class ResolveBlobProvider : IPipelineProcessor
    {
        public void Process(PipelineArg pipelineArg)
        {
            var arg = (ConfigureServicesArg)pipelineArg;
            var serviceCollection = arg.ServicesCollection;
            serviceCollection.AddSingleton(typeof(IBlobProvider), serviceProvider =>
            {
                var applicationConfiguration = serviceProvider.GetService<ApplicationConfiguration>();
                var configXml = applicationConfiguration.ConfigurationXml;
                var blobNode = configXml.SelectSingleNode("/configuration/blob");
                if (blobNode == null)
                {
                    return new DummyBlobProvider();
                }
                var blobProviderType = Type.GetType(blobNode.Attributes!["provider"]!.Value);
                var parameters = new List<object>();
                foreach (XmlNode? paramNode in blobNode.ChildNodes)
                {
                    parameters.Add(paramNode?.InnerText!);
                }
                using var scope = ServiceActivator.GetScope();
                var blobProvider = (IBlobProvider)ActivatorUtilities.CreateInstance(scope.ServiceProvider, blobProviderType, parameters.ToArray());
                blobProvider.Initialize();
                return blobProvider;
            });
        }
    }
}
