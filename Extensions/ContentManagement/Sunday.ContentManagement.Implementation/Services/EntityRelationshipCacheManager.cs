using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Extensions;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Application;
using Sunday.Core.Domain.Interfaces;
using Sunday.Core.Extensions;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Services
{
    [ServiceTypeOf(typeof(IEntityCacheManager), LifetimeScope.Singleton)]
    public class EntityRelationshipCacheManager : IEntityCacheManager
    {
        private readonly ICacheEngine _cacheEngine;
        private readonly ICacheKeyCreator _cacheKeyCreator;
        private readonly ConcurrentDictionary<string, List<string>> _cacheDependencies = new ConcurrentDictionary<string, List<string>>();

        public EntityRelationshipCacheManager(ICacheEngine cacheEngine, ICacheKeyCreator cacheKeyCreator)
        {
            _cacheEngine = cacheEngine;
            _cacheKeyCreator = cacheKeyCreator;
        }

        public async Task Set<T>(T value, TimeSpan? expiration = null) where T : IEntity
        {
            var cacheKey = _cacheKeyCreator.GetCacheKey<T>(value.Id);
            var dependants = await GetDependants(value);
            foreach (var key in dependants)
            {
                _cacheEngine.Evict(key);
            }
            _cacheDependencies.AppendIfNotExist(cacheKey, dependants);
            var masters = await GetMasters(value);
            masters.Iter(key => _cacheDependencies.AppendIfNotExist(key, cacheKey));
            _cacheEngine.Set(cacheKey, value, expiration);
        }

        public async Task<Option<T>> ReadThrough<T>(Guid id, Func<Task<Option<T>>> creator, TimeSpan? expiration = null) where T : IEntity
        {
            var entry = Get<T>(id);
            if (entry.IsSome) return entry;
            var data = await creator();
            if (data.IsSome)
            {
                await Set(data.Get());
            }
            return data;
        }

        public Option<T> Get<T>(Guid id) where T : IEntity
        {
            var cacheKey = _cacheKeyCreator.GetCacheKey<T>(id);
            var entry = _cacheEngine.Get<T>(cacheKey);
            if (entry.IsNone) return Option<T>.None;
            return entry.Get();
        }

        public async Task Remove(IEntity entity)
        {
            var cacheKey = _cacheKeyCreator.GetCacheKey(entity);
            var dependants = await GetDependants(entity);
            foreach (var key in dependants)
            {
                _cacheEngine.Evict(key);
            }
            _cacheEngine.Evict(cacheKey);
        }

        private async Task<string[]> GetDependants(IEntity entity)
        {
            var cacheKey = _cacheKeyCreator.GetCacheKey(entity);
            var dependants = _cacheDependencies.Get(cacheKey).IfNone(new List<string>());
            var arg = new GetEntityDependantsArg(entity);
            await ApplicationPipelines.RunAsync("cms.entity.getDependants", arg);
            dependants.AddRange(arg.Dependants.Where(d => !dependants.Contains(d)));
            return dependants.ToArray();
        }

        private async Task<string[]> GetMasters(IEntity entity)
        {
            var arg = new GetEntityMastersArg(entity);
            await ApplicationPipelines.RunAsync("cms.entity.getMasters", arg);
            return arg.Masters.ToArray();
        }
    }
}
