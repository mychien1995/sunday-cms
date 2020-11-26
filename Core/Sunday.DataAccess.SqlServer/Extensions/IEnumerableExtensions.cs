using System.Collections.Generic;
using System.Linq;

namespace Sunday.DataAccess.SqlServer.Extensions
{
    public static class EnumerableExtensions
    {
        public static string ToDatabaseList<T>(this IEnumerable<T> list) => string.Join("|", list);

        public static string ToDatabaseDictionary(this Dictionary<string, string> list) =>
            string.Join("|", list.Select(kv => $"{kv.Key}:{kv.Value}"));
    }
}
