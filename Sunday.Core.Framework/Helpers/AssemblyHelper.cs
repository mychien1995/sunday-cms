using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Loader;

namespace Sunday.Core.Framework.Helpers
{
    public static class AssemblyHelper
    {
        public static IEnumerable<Assembly> GetAllAssemblies(Expression<Func<string, bool>> predicate)
        {
            var assemblyPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(assemblyPath);
            var compiled = predicate.Compile();
            var result = new List<Assembly>();
            foreach (string dll in Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly))
            {
                if (compiled.Invoke(dll))
                {
                    try
                    {
                        Assembly loadedAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
                        result.Add(loadedAssembly);
                    }
                    catch (Exception ex)
                    {

                    }
                }
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
                        if (!list.Contains(reference.FullName))
                        {
                            stack.Push(Assembly.Load(reference));
                            list.Add(reference.FullName);
                        }
                    }

            }
            while (stack.Count > 0);

        }

        public static IEnumerable<Type> GetClasses(Assembly assembly, Expression<Func<Type, bool>> predicate)
        {
            var compiled = predicate.Compile();
            foreach (Type type in assembly.GetTypes())
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
                foreach (Type type in assembly.GetTypes())
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
                foreach (Type type in assembly.GetTypes())
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
