using System.Runtime.CompilerServices;

namespace System.Reflection
{
    public static class MethodsExtensions
    {
        public static bool IsAsyncMethod(this MethodInfo method)
        {
            Type attType = typeof(AsyncStateMachineAttribute);
            var attrib = (AsyncStateMachineAttribute)method.GetCustomAttribute(attType);
            return (attrib != null);
        }
    }
}
