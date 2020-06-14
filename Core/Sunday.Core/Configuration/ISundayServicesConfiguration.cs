using Microsoft.Extensions.DependencyInjection;

namespace Sunday.Core.Configuration
{
    public interface ISundayServicesConfiguration
    {
        IServiceCollection Services { get; }
    }
}
