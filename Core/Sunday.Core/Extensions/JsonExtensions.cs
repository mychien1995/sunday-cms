using System;
using Newtonsoft.Json;

namespace Sunday.Core.Extensions
{
    public static class JsonExtensions
    {
        public static bool TryJsonParse<T>(this string stringValue, out T value)
        {
            try
            {
                value = JsonConvert.DeserializeObject<T>(stringValue);
            }
            catch (Exception ex)
            {
                value = default(T)!;
                return false;
            }
            return true;
        }
    }
}
