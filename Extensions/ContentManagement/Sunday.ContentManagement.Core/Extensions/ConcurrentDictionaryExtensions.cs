using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Sunday.ContentManagement.Extensions
{
    public static class ConcurrentDictionaryExtensions
    {
        public static void AppendIfNotExist(this ConcurrentDictionary<string, List<string>> dictionary, string key,
            IEnumerable<string> values)
        {
            var tmp = values.Distinct().ToArray();
            if (dictionary.TryGetValue(key, out var val))
            {
                val!.AddRange(tmp.Where(v => val.Contains(v)));
            }
            else val = tmp.ToList();
            dictionary[key] = val;
        }

        public static void AppendIfNotExist(this ConcurrentDictionary<string, List<string>> dictionary, string key,
            string value)
            => dictionary.AppendIfNotExist(key, new[] {value});
    }
}
