using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core.Domain.Interfaces;

namespace Sunday.Core.Application
{
    public interface IEntityCacheManager
    {
        Task Set<T>(T value, TimeSpan? expiration) where T : IEntity;

        Option<T> Get<T>(Guid id) where T : IEntity;

        Task Remove(IEntity entity);
    }
}
