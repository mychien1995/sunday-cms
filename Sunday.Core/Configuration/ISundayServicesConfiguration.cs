using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Configuration
{
    public interface ISundayServicesConfiguration
    {
        IServiceCollection Services { get; }
    }
}
