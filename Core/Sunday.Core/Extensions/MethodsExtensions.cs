using System.Runtime.CompilerServices;

namespace System.Reflection
{
    public static class MethodsExtensions
    {
        public static bool IsAsyncMethod(this MethodInfo method)
        {
            var attType = typeof(AsyncStateMachineAttribute);
            var attribute = (AsyncStateMachineAttribute)method.GetCustomAttribute(attType);
            return (attribute != null);
        }
    }
}
