using System.Collections.Generic;

namespace Sunday.DataAccess.SqlServer.Extensions
{
    public static class EnumerableExtensions
    {
        public static string ToDatabaseList<T>(this IEnumerable<T> list) => string.Join("|", list);
    }
}
