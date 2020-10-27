using System.Collections.Generic;
using System.Linq;

namespace Sunday.Foundation.Persistence.Extensions
{
    public static class StringExtensions
    {
        public static List<string> ToStringList(this string list) => list.Split('|').ToList();
    }
}
