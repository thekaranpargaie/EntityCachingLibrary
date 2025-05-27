using EntityCachingLib.Interfaces;
using EntityCachingLib.Models;
using Microsoft.Extensions.Caching.Memory;

namespace EntityCachingLib.Services
{
    public class MemoryCacheService : ICacheService
    {
        public CacheProvider Provider => CacheProvider.Memory;
        private readonly IMemoryCache _cache;
        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        private static string GetCacheKey<T>(string key)
            => $"{typeof(T).Name}_{key}";

        public Task<T?> GetAsync<T>(string key)
        {
            var cacheKey = GetCacheKey<T>(key);
            _cache.TryGetValue(cacheKey, out T? value);
            return Task.FromResult(value);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? ttl = null)
        {
            var cacheKey = GetCacheKey<T>(key);
            _cache.Set(cacheKey, value, ttl ?? TimeSpan.FromMinutes(10));
            return Task.CompletedTask;
        }

        public Task RemoveAsync<T>(string key)
        {
            var cacheKey = GetCacheKey<T>(key);
            _cache.Remove(cacheKey);
            return Task.CompletedTask;
        }
    }
}