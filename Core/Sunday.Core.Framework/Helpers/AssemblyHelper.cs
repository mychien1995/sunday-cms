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
        public static IEnumerable<Assembly> GetAssemblies(Expression<Func<AssemblyName, bool>> predicate)
        {
            var list = new List<string>();
            var stack = new Stack<Assembly>();
            var compiled = predicate.Compile();
            stack.Push(Assembly.GetEntryAssembly());

            do
            {
                var asm = stack.Pop();

                yield return asm;

                foreach (var reference in asm.GetReferencedAssemblies())
                    if (compiled.Invoke(reference))
                    {
                        if (list.Contains(reference.FullName)) continue;
                        stack.Push(Assembly.Load(reference));
                        list.Add(reference.FullName);
                    }

            }
            while (stack.Count > 0);

        }

        public static IEnumerable<Type> GetClasses(Assembly assembly, Expression<Func<Type, bool>> predicate)
        {
            var compiled = predicate.Compile();
            foreach (var type in assembly.GetTypes())
            {
                if (compiled.Invoke(type))
                {
                    yield return type;
                }
            }
        }

        public static IEnumerable<Type> GetClasses(Assembly[] assemblies, Expression<Func<Type, bool>> predicate)
        {
            var compiled = predicate.Compile();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (compiled.Invoke(type))
                    {
                        yield return type;
                    }
                }
            }
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
