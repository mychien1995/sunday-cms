using System;
using LanguageExt;
using Microsoft.Extensions.Caching.Memory;
using Sunday.Core.Application;

namespace Sunday.Core.Implementation
{
    public class InMemoryCacheEngine : ICacheEngine
    {
        private readonly IMemoryCache _cache;
        private readonly object _lock = new object();

        public InMemoryCacheEngine(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Set<T>(string key, T value, TimeSpan? expiration)
        {
            lock (_lock)
            {
                if (expiration.HasValue)
                    _cache.Set(key, value, expiration.Value);
                else _cache.Set(key, value);
            }
        }

        public Option<T> Get<T>(string key)
        {
            var item = _cache.Get<T>(key);
            if (item != null) return item;
            lock (_lock)
            {
                item = _cache.Get<T>(key);
                return item ?? Option<T>.None;
            }
        }
    }
}
