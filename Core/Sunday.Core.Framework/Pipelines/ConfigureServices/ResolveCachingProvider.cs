using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sunday.Core.Application;
using Sunday.Core.Configuration;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.Core.Framework.Pipelines.ConfigureServices
{
    public class ResolveCachingProvider : IPipelineProcessor
    {
        private readonly ILogger<ResolveCachingProvider> _logger;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public ResolveCachingProvider(ILogger<ResolveCachingProvider> logger, ApplicationConfiguration applicationConfiguration)
        {
            _logger = logger;
            _applicationConfiguration = applicationConfiguration;
        }

        public void Process(PipelineArg pipelineArg)
        {
            var arg = (ConfigureServicesArg)pipelineArg;
            var configXml = _applicationConfiguration.ConfigurationXml;
            var cachingNode = configXml.SelectSingleNode("/configuration/caching");
            if (cachingNode == null)
            {
                _logger.LogInformation("No caching provider found");
                return;
            }
            var provider = cachingNode!.Attributes!["provider"].Value;
            _logger.LogInformation($"Caching provider is {provider}");
            arg.ServicesCollection.AddSingleton<ICacheEngine>(sp =>
            {
                var type = Type.GetType(provider);
                var cachingProvider =
                    (ICacheEngine)ActivatorUtilities.CreateInstance(sp, type);
                return cachingProvider;
            });
        }
    }
}
