using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Sunday.Core.Configuration;
using Sunday.Core.Configuration.Nodes;
using Sunday.Core.Framework.Helpers;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;
using XmlDocumentMerger;

namespace Sunday.Core.Framework
{
    public static class ServicesCollectionExtensions
    {
        private static readonly ILogger Logger = Log.Logger;
        private static ConfigurationNode _configuration = new ConfigurationNode();
        public static ISundayServicesConfiguration Sunday(this IServiceCollection services)
        {
            return new SundayServicesConfiguration(services);
        }

        public static ISundayServicesConfiguration LoadConfiguration(this ISundayServicesConfiguration services, IWebHostEnvironment hostingEnv, IConfiguration configuration)
        {
            Logger.Information("Start loading configuration");
            string configFolderPath;
            var configPath = configuration.GetValue<string>("SundayConfigPath");
            if (!string.IsNullOrEmpty(configPath))
            {
                configFolderPath = configPath;
            }
            else
            {
                var configurationPath = "\\config";
                configFolderPath = hostingEnv.WebRootPath + configurationPath;
            }
            Logger.Information($"Config path specified at {configFolderPath}");
            var filePath = configFolderPath + "\\sunday.config";
            if (!File.Exists(filePath))
            {
                Logger.Warning($"Main config path {filePath} not found !");
                return services;
            }
            var configFileContent = File.ReadAllText(filePath);
            var includeFolder = configFolderPath + "\\include";
            var includeFiles = Directory.GetFiles(includeFolder, "*.config");
            configFileContent = includeFiles.Select(File.ReadAllText)
                .Aggregate(configFileContent, (current, xmlContent) => XmlMerger.MergeDocuments(xmlContent, current));
            var serializer = new XmlSerializer(typeof(ConfigurationNode));
            using TextReader reader = new StringReader(configFileContent);
            var configurationNode = (ConfigurationNode)serializer.Deserialize(reader);
            _configuration = configurationNode;
            var document = new XmlDocument();
            document.LoadXml(configFileContent);
            services.Services.AddSingleton(new ApplicationConfiguration(configurationNode, document));
            AddSetting(configurationNode);
            AddPipelines(document);
            return services;
        }

        public static IApplicationBuilder InitializeSunday(this IApplicationBuilder application)
        {
            Logger.Information("Sunday Initializing...");
            ApplicationPipelines.RunAsync("initialize", new InitializationArg(application)).Wait();
            Logger.Information("Sunday Initialization Completed!");
            return application;
        }

        public static ISundayServicesConfiguration LoadServices(this ISundayServicesConfiguration serviceConf)
        {
            Logger.Information("Start loading CMS Services");
            var services = serviceConf.Services;
            var assemblies = AssemblyHelper.GetAllAssemblies(x => (!x.StartsWith("api-") && !x.StartsWith("Microsoft") && !x.StartsWith("System")) &&
            (x.Contains("Sunday") || x.Contains("Plugin"))).ToArray();

            var types = AssemblyHelper.GetClassesWithAttribute(assemblies, typeof(ServiceTypeOfAttribute));
            foreach (var type in types)
            {
                var attr = ((ServiceTypeOfAttribute)type.GetCustomAttribute(typeof(ServiceTypeOfAttribute))!);
                var parentType = attr.ServiceType;
                var scope = attr.LifetimeScope;
                switch (scope)
                {
                    case LifetimeScope.Transient:
                        if (parentType == type)
                            services.AddTransient(type);
                        else services.AddTransient(parentType, type);
                        break;
                    case LifetimeScope.Singleton:
                        if (parentType == type)
                            services.AddSingleton(type);
                        else services.AddSingleton(parentType, type);
                        break;
                    case LifetimeScope.PerRequest:
                        if (parentType == type)
                            services.AddScoped(type);
                        else services.AddScoped(parentType, type);
                        break;
                    default:
                        if (parentType == type)
                            services.AddTransient(type);
                        else services.AddTransient(parentType, type);
                        break;
                }
            }
            AddConfiguredServices(_configuration, services);
            ServiceActivator.Configure(services.BuildServiceProvider());
            ApplicationPipelines.RunAsync("configureServices", new ConfigureServicesArg(services)).Wait();
            ServiceActivator.Configure(services.BuildServiceProvider());
            return serviceConf;
        }

        private static void AddConfiguredServices(ConfigurationNode configurationNode, IServiceCollection serviceCollection)
        {
            if (configurationNode?.Services == null || !configurationNode.Services.Any())
                return;
            foreach (var service in configurationNode.Services.Where(x => !string.IsNullOrEmpty(x?.ServiceType) || !string.IsNullOrEmpty(x?.ImplementationType)))
            {
                var serviceType = Type.GetType(service.ServiceType!);
                var implementType = Type.GetType(service.ImplementationType!);
                switch (service.LifetimeScope)
                {
                    case "Singleton":
                        serviceCollection.AddSingleton(serviceType, implementType);
                        break;
                    case "Scope":
                        serviceCollection.AddScoped(serviceType, implementType);
                        break;
                    default:
                        serviceCollection.AddTransient(serviceType, implementType);
                        break;
                }
            }
        }
        private static void AddSetting(ConfigurationNode configurationNode)
        {
            if (configurationNode?.Settings == null || !configurationNode.Settings.Any())
            {
                Logger.Information("Start loading CMS Settings. No values found");
                return;
            }
            var settings = configurationNode.Settings.Where(x => !string.IsNullOrEmpty(x?.Key)).ToArray();
            Logger.Information($"Start loading CMS Settings. {settings.Length} values found");
            settings.Iter(setting => ApplicationSettings.Set(setting.Key!, setting.Value!));
        }

        private static void AddPipelines(XmlDocument document)
        {
            ApplicationPipelines.Initialize(document);
        }
    }
}
