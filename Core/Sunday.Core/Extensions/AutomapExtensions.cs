using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Sunday.Core
{
    public static class AutomapExtensions
    {
        public static T MapTo<T>(this object obj)
        {
            using (var scope = ServiceActivator.GetScope())
            {
                var mapper = scope.ServiceProvider.GetService<IMapper>();
                return mapper.Map<T>(obj);
            }
        }
    }
}
