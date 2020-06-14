using Microsoft.Extensions.DependencyInjection;

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
