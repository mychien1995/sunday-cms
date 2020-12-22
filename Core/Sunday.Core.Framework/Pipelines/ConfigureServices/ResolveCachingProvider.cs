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
        private static ICacheEngine? _cacheEngine;
        public void Process(PipelineArg pipelineArg)
        {
            var arg = (ConfigureServicesArg)pipelineArg;
            arg.ServicesCollection.AddSingleton(sp =>
            {
                if (_cacheEngine != null) return _cacheEngine;
                var logger = sp.GetService<ILogger<ResolveCachingProvider>>();
                var applicationConfiguration = sp.GetService<ApplicationConfiguration>();
                var configXml = applicationConfiguration.ConfigurationXml;
                var cachingNode = configXml.SelectSingleNode("/configuration/caching");
                if (cachingNode == null)
                {
                    logger.LogInformation("No caching provider found");
                    return null;
                }
                var provider = cachingNode!.Attributes!["provider"].Value;
                logger.LogInformation($"Caching provider is {provider}");
                var cachingProviderType = Type.GetType(provider);
                _cacheEngine = (ICacheEngine)ActivatorUtilities.CreateInstance(sp, cachingProviderType);
                return _cacheEngine;
            });
        }
    }
}
