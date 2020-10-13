using System;
using LanguageExt;

namespace Sunday.Core.Extensions
{
    public static class OptionExtensions
    {
        public static T Get<T>(this Option<T> opt)
        {
            return opt.GetOrThrow<T>(() => new InvalidOperationException());
        }

        public static T GetOrThrow<T>(this Option<T> opt, Func<Exception> noneHandler)
        {
            return opt.IfNone(() => throw noneHandler());
        }
    }
}
