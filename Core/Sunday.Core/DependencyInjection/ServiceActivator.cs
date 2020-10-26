﻿using Microsoft.Extensions.DependencyInjection;
using System;

// ReSharper disable once CheckNamespace
namespace Sunday.Core
{
    public class ServiceActivator
    {
        internal static IServiceProvider ServiceProvider = null!;

        /// <summary>
        /// Configure ServiceActivator with full serviceProvider
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Configure(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// Create a scope where use this ServiceActivator
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceScope GetScope(IServiceProvider? serviceProvider = null)
        {
            var provider = serviceProvider ?? ServiceProvider;
            return provider
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
        }
    }
}
