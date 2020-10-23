using System.Collections.Generic;
using LanguageExt;

namespace Sunday.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static Option<T> Get<TKey, T>(this IDictionary<TKey, T> dict, TKey key) where TKey : notnull
        {
            return dict.TryGetValue<TKey, T>(key);
        }
    }
}
