using Sunday.DataAccess.SqlServer.Attributes;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace System
{
    public static class DapperExtensions
    {
        public static object ToDapperParameters(this object obj)
        {
            var parameters = new ExpandoObject();
            var properties = obj.GetType().GetProperties();
            foreach (var prop in properties)
            {
                if (prop.GetCustomAttribute<DapperIgnoreParamAttribute>(true) == null)
                {
                    parameters.TryAdd(prop.Name, prop.GetValue(obj));
                }
            }
            return parameters;
        }
    }
}
