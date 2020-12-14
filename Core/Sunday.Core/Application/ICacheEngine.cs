using System;
using LanguageExt;

namespace Sunday.Core.Application
{
    public interface ICacheEngine
    {
        void Set<T>(string key, T value, TimeSpan? expiration);

        Option<T> Get<T>(string key);

        void Evict(string key);
    }
}
