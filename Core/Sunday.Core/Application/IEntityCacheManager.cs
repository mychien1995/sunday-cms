using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core.Domain.Interfaces;

namespace Sunday.Core.Application
{
    public interface IEntityCacheManager
    {
        Task Set<T>(T value, TimeSpan? expiration = null) where T : IEntity;

        Option<T> Get<T>(Guid id) where T : IEntity;

        Task Remove(IEntity entity);

        Task<Option<T>> ReadThrough<T>(Guid id, Func<Task<Option<T>>> creator, TimeSpan? expiration = null)
            where T : IEntity;
    }
}
