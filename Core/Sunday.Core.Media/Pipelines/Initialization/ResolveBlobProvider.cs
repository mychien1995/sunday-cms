using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sunday.Core.Configuration;
using Sunday.Core.Media.Application;
using Sunday.Core.Media.Implementation;
using Sunday.Core.Pipelines.Arguments;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Sunday.Core.Media.Pipelines.Initialization
{
    public class ResolveBlobProvider
    {
        public void Process(ConfigureServicesArg arg)
        {
            var serviceCollection = arg.ServicesCollection;
            serviceCollection.AddSingleton(typeof(IBlobProvider), serviceProvider =>
            {
                var applicationConfiguration = serviceProvider.GetService<ApplicationConfiguration>();
                var webHostEnv = serviceProvider.GetService<IWebHostEnvironment>();
                var configXml = applicationConfiguration.ConfigurationXml;
                var blobNode = configXml.SelectSingleNode("/configuration/blob");
                if (blobNode == null)
                {
                    var fileBlobProvider = new FileBlobProvider(webHostEnv, "/Images", true);
                    fileBlobProvider.Initialize();
                    return fileBlobProvider;
                }

                var blobProviderType = Type.GetType(blobNode.Attributes["provider"].Value);
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
