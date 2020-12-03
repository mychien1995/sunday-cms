using System;
using System.Threading.Tasks;

namespace Sunday.Core.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<T2> MapResultTo<T, T2>(this Task<T> task, Func<T, T2> mapper)
            => mapper(await task);

        public static async Task ThenDo<T>(this Task<T> task, Action<T> mapper)
            => mapper(await task);
    }
}
