using System;
using Sunday.Core;
using Sunday.Core.Domain.Interfaces;

namespace Sunday.ContentManagement.Services
{
    public interface ICacheKeyCreator
    {
        string GetCacheKey(IEntity entity);

        string GetCacheKey(Type type, Guid id);

        string GetCacheKey<T>(Guid id);
    }

    [ServiceTypeOf(typeof(ICacheKeyCreator))]
    public class DefaultCacheKeyCreator : ICacheKeyCreator
    {
        public string GetCacheKey(IEntity entity)
            => GetCacheKey(entity.GetType(), entity.Id);

        public string GetCacheKey(Type type, Guid id)
            => $"{type.Name}:{id}";

        public string GetCacheKey<T>(Guid id)
            => GetCacheKey(typeof(T), id);
    }
}
