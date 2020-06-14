using System;
using System.Collections.Concurrent;

namespace Sunday.Core
{
    public class ApplicationSettings
    {
        private static ConcurrentDictionary<string, string> _settings = new ConcurrentDictionary<string, string>();

        public static void Set(string key, string value)
        {
            if (_settings.ContainsKey(key))
            {
                _settings.TryRemove(key, out string tmp);
            }
            _settings.TryAdd(key, value);
        }

        public static string Get(string key)
        {
            if (_settings.TryGetValue(key, out string value)) return value;
            return null;
        }
        public static T Get<T>(string key)
        {
            var value = Get(key);
            if (value == null) return default(T);
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
