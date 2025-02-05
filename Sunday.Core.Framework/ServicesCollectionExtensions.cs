﻿using Sunday.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Sunday.Core;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Sunday.Core.Framework.Helpers;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesCollectionExtensions
    {
        public static ISundayServicesConfiguration Sunday(this IServiceCollection services)
        {
            return new SundayServicesConfiguration(services);
        }

        public static ISundayServicesConfiguration LoadConfiguration(this ISundayServicesConfiguration services, IWebHostEnvironment hostingEnv, IConfiguration configuration)
        {
            var environment = configuration.GetValue<string>("Environment");
            var configurationPath = string.IsNullOrEmpty(environment) ? $"\\config\\sunday.config" : $"\\config\\sunday.{environment.ToLower()}.config";
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var configFileContent = File.ReadAllText(hostingEnv.WebRootPath + configurationPath);
                var serializer = new XmlSerializer(typeof(ConfigurationNode));
                using (TextReader reader = new StringReader(configFileContent))
                {
                    ConfigurationNode configurationNode = (ConfigurationNode)serializer.Deserialize(reader);
                    AddSetting(configurationNode);
                    AddPipelines(configurationNode);
                }
            }
            return services;
        }

        public static ISundayServicesConfiguration Initialize(this ISundayServicesConfiguration services)
        {
            ApplicationPipelines.Run("initialize", null);
            return services;
        }

        public static ISundayServicesConfiguration LoadServices(this ISundayServicesConfiguration serviceConf)
        {
            var services = serviceConf.Services;
            var assemblies = AssemblyHelper.GetAllAssemblies(x => (!x.StartsWith("Microsoft") && !x.StartsWith("System")) &&
            (x.Contains("Sunday") || x.Contains("Plugin"))).ToArray();

            var types = AssemblyHelper.GetClassesWithAttribute(assemblies, typeof(ServiceTypeOfAttribute));
            foreach (var type in types)
            {
                var attr = ((ServiceTypeOfAttribute)type.GetCustomAttribute(typeof(ServiceTypeOfAttribute)));
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
            var serviceProvider = services.BuildServiceProvider();
            ServiceActivator.Configure(serviceProvider);
            return serviceConf;
        }

        private static void AddSetting(ConfigurationNode configurationNode)
        {
            if (configurationNode == null || configurationNode.Settings == null || !configurationNode.Settings.Any())
                return;
            foreach (var setting in configurationNode.Settings.Where(x => !string.IsNullOrEmpty(x?.Key)))
            {
                ApplicationSettings.Set(setting.Key, setting.Value);
            }
        }

        private static void AddPipelines(ConfigurationNode configurationNode)
        {
            ApplicationPipelines.Initialize(configurationNode);
        }
    }
}
