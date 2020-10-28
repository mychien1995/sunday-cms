using System.Collections.Generic;
using System.Linq;

namespace Sunday.DataAccess.SqlServer.Extensions
{
    public static class StringExtensions
    {
        public static List<string> ToStringList(this string list) => list.Split('|').ToList();
    }
}
