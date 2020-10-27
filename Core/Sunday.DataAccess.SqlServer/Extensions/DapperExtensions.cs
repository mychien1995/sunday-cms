using Sunday.DataAccess.SqlServer.Attributes;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace System
{
    public static class DapperExtensions
    {
        public static object ToDapperParameters(this object obj, DbOperation dbOperation = DbOperation.All)
        {
            var parameters = new ExpandoObject();
            var properties = obj.GetType().GetProperties();
            foreach (var prop in properties)
            {
                if (prop.GetCustomAttribute<DapperIgnoreParamAttribute>(true) != null)
                {
                    var op = prop.GetCustomAttribute<DapperIgnoreParamAttribute>(true)!.DbOperation;
                    if(op == dbOperation || op == DbOperation.All) continue;
                }
                var name = prop.Name;
                if (prop.GetCustomAttribute<DapperParamAttribute>() != null)
                {
                    name = prop.GetCustomAttribute<DapperParamAttribute>()!.ParamName;
                }
                parameters.TryAdd(name, prop.GetValue(obj));
            }
            return parameters;
        }
    }
}
