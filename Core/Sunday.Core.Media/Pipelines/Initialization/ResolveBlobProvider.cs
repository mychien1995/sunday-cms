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
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ResolveBlobProvider(ApplicationConfiguration applicationConfiguration, IWebHostEnvironment webHostEnvironment)
        {
            _applicationConfiguration = applicationConfiguration;
            _webHostEnvironment = webHostEnvironment;
        }
        public void Process(InitializationArg arg)
        {
            var configXml = _applicationConfiguration.ConfigurationXml;
            var blobNode = configXml.SelectSingleNode("/configuration/blob");
            if (blobNode == null)
            {
                var fileBlobProvider = new FileBlobProvider(_webHostEnvironment, "/Images", "true");
                fileBlobProvider.Initialize();
                arg.ServiceCollection.AddSingleton(typeof(IBlobProvider), fileBlobProvider);
            }
            else
            {
                var blobProviderType = Type.GetType(blobNode.Attributes["provider"].Value);
                var parameters = new List<object>();
                foreach (XmlNode paramNode in blobNode.ChildNodes)
                {
                    parameters.Add(paramNode.InnerText);
                }
                using (var scope = ServiceActivator.GetScope())
                {
                    var blobProvider = (IBlobProvider)ActivatorUtilities.CreateInstance(scope.ServiceProvider, blobProviderType, parameters.ToArray());
                    blobProvider.Initialize();
                    arg.ServiceCollection.AddSingleton(typeof(IBlobProvider), blobProvider);
                }
            }
            var serviceProvider = arg.ServiceCollection.BuildServiceProvider();
            ServiceActivator.Configure(serviceProvider);
        }
    }
}
