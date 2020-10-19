using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Loader;

namespace Sunday.Core.Framework.Helpers
{
    public static class AssemblyHelper
    {
        public static IEnumerable<Assembly> GetAllAssemblies(Expression<Func<string, bool>> predicate)
        {
            var assemblyPath = Assembly.GetEntryAssembly()?.Location;
            var directory = System.IO.Path.GetDirectoryName(assemblyPath);
            var compiled = predicate.Compile();
            var result = new List<Assembly>();
            foreach (var dll in Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly))
            {
                if (!compiled.Invoke(dll)) continue;
                var loadedAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
                result.Add(loadedAssembly);
            }
            return result;
        }

        public static IEnumerable<Type> GetClassesWithAttribute(Assembly[] assemblies, Type attributeType)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.GetCustomAttributes(attributeType, true).Length > 0)
                    {
                        yield return type;
                    }
                }
            }
        }
    }
}
