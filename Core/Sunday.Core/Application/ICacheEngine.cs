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

    public class DummyCacheEngine : ICacheEngine
    {
        public void Set<T>(string key, T value, TimeSpan? expiration)
        {
        }

        public Option<T> Get<T>(string key)
        {
            return Option<T>.None;
        }

        public void Evict(string key)
        {
        }
    }
}
