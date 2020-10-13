using System;
using System.Threading.Tasks;

namespace Sunday.Core.Extensions
{
    public static class TaskExtensions
    {
        public static Task<T2> MapResultTo<T, T2>(this Task<T> task, Func<T, T2> mapper)
            => task.ContinueWith(t => mapper(t.Result));
    }
}
