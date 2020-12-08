using System;
using System.Collections.Concurrent;

// ReSharper disable once CheckNamespace
namespace Sunday.Core
{
    public class ApplicationSettings
    {
        private static readonly ConcurrentDictionary<string, string> _settings = new ConcurrentDictionary<string, string>();

        public static void Set(string key, string value)
        {
            if (_settings.ContainsKey(key))
            {
                _settings.TryRemove(key, out var _);
            }
            _settings.TryAdd(key, value);
        }

        public static string? Get(string key)
        {
            return _settings.TryGetValue(key, out var value) ? value : null;
        }
        public static T Get<T>(string key)
        {
            var value = Get(key);
            if (value == null) return default!;
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
