using EntityCachingLib.Interfaces;
using EntityCachingLib.Models;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace EntityCachingLib.Services
{
    public class RedisCacheService : ICacheService
    {
        public CacheProvider Provider => CacheProvider.Redis;
        private readonly IDatabase _db;
        public RedisCacheService(IOptions<CacheConfig> cacheConfig)
        {
            var connection = ConnectionMultiplexer.Connect(cacheConfig.Value.Redis.ConnectionString);
            _db = connection.GetDatabase();
        }

        private static string GetCacheKey<T>(string key)
            => $"{typeof(T).Name}_{key}";

        public async Task<T?> GetAsync<T>(string key)
        {
            var cacheKey = GetCacheKey<T>(key);
            var value = await _db.StringGetAsync(cacheKey);
            if (value.IsNullOrEmpty) return default;
            return JsonSerializer.Deserialize<T>(value!);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? ttl = null)
        {
            var cacheKey = GetCacheKey<T>(key);
            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(cacheKey, json, ttl ?? TimeSpan.FromMinutes(10));
        }

        public async Task RemoveAsync<T>(string key)
        {
            var cacheKey = GetCacheKey<T>(key);
            await _db.KeyDeleteAsync(cacheKey);
        }
    }
}