using System;
using Sunday.Core.Domain.Interfaces;

namespace Sunday.ContentManagement.Services
{
    public interface ICacheKeyCreator
    {
        string GetCacheKey(IEntity entity)
            => GetCacheKey(entity.GetType(), entity.Id);

        string GetCacheKey(Type type, Guid id)
            => $"{type.Name}:{id}";

        string GetCacheKey<T>(Guid id)
            => GetCacheKey(typeof(T), id);
    }
}
