using System.Collections.Generic;
using System.Linq;

namespace Sunday.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TDes> CastListTo<TDes>(this IEnumerable<object> list) where TDes : class
            => list.Select(item => item!.MapTo<TDes>());
    }
}
