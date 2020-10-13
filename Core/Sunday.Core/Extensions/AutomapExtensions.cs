using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Sunday.Core.Extensions
{
    public static class AutomapExtensions
    {
        public static T MapTo<T>(this object obj) where T : class
        {
            using var scope = ServiceActivator.GetScope();
            var mapper = scope.ServiceProvider.GetService<IMapper>();
            return mapper.Map<T>(obj);
        }
    }
}
