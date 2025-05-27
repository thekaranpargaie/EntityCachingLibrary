using EntityCachingLib.Interfaces;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EntityCachingLib.Services
{
    public class EntityCacheManager<TEntity, TKey> : IEntityCacheManager<TEntity, TKey> where TEntity : class
    {
        private readonly ICacheServiceFactory _cacheFactory;
        private readonly string _prefix;
        private readonly TimeSpan _ttl;

        public EntityCacheManager(ICacheServiceFactory cacheFactory, string prefix = "", TimeSpan? ttl = null)
        {
            _cacheFactory = cacheFactory;
            _prefix = string.IsNullOrWhiteSpace(prefix) ? typeof(TEntity).Name : prefix;
            _ttl = ttl ?? TimeSpan.FromMinutes(10);
        }

        private string GetCacheKey(TKey id) => $"{_prefix}:{id}";

        public async Task<TEntity?> GetAsync(TKey id, Func<TKey, Task<TEntity?>> fetchFromDb)
        {
            var key = GetCacheKey(id);
            // Try primary first
            var primary = _cacheFactory.GetPrimary();
            var cached = await primary.GetAsync<TEntity>(key);
            if (cached is not null) return cached;

            // Try other enabled caches
            foreach (var cache in _cacheFactory.GetAllExceptPrimary())
            {
                cached = await cache.GetAsync<TEntity>(key);
                if (cached is not null)
                {
                    // Optionally, update primary cache
                    await primary.SetAsync(key, cached, _ttl);
                    return cached;
                }
            }

            // Fetch from DB
            var entity = await fetchFromDb(id);
            if (entity != null)
            {
                var tasks = new List<Task>();
                foreach (var cache in _cacheFactory.GetAllEnabled())
                    tasks.Add(cache.SetAsync(key, entity, _ttl));
                await Task.WhenAll(tasks);
            }
            return entity;
        }

        public async Task SetAsync(TKey id, TEntity entity)
        {
            var key = GetCacheKey(id);
            var tasks = new List<Task>();
            foreach (var cache in _cacheFactory.GetAllEnabled())
                tasks.Add(cache.SetAsync(key, entity, _ttl));
            await Task.WhenAll(tasks);
        }

        public async Task RemoveAsync<TEntity>(TKey id)
        {
            var key = GetCacheKey(id);
            var tasks = new List<Task>();
            foreach (var cache in _cacheFactory.GetAllEnabled())
                tasks.Add(cache.RemoveAsync<TEntity>(key));
            await Task.WhenAll(tasks);
        }
    }
}