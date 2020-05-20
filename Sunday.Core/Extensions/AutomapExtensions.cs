using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core
{
    public static class AutomapExtensions
    {
        public static T MapTo<T>(this object obj)
        {
            using(var scope = ServiceActivator.GetScope())
            {
                var mapper = scope.ServiceProvider.GetService<IMapper>();
                return mapper.Map<T>(obj);
            }
        }
    }
}
