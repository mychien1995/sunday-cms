using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Configuration
{
    public class SundayServicesConfiguration : ISundayServicesConfiguration
    {
        public SundayServicesConfiguration(IServiceCollection services)
        {
            this.Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
