using System.Collections.Generic;
using System.Linq;

namespace Sunday.DataAccess.SqlServer.Extensions
{
    public static class StringExtensions
    {
        public static List<string> ToStringList(this string list) => string.IsNullOrWhiteSpace(list) ? new List<string>() : list.Split('|').ToList();

        public static Dictionary<string, string> ToDictionary(this string dictionary)
            => string.IsNullOrWhiteSpace(dictionary)
                ? new Dictionary<string, string>()
                : dictionary.Split('|').Select(
                    part =>
                    {
                        var kv = part.Split(':');
                        return (kv[0], kv[1]);
                    }).ToDictionary(k => k.Item1, v => v.Item2);
    }
}
