using EntityCachingLib.Models;
using System;
using System.Threading.Tasks;

namespace EntityCachingLib.Interfaces
{
    public interface ICacheService
    {
        public CacheProvider Provider { get; }
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? ttl = null);
        Task RemoveAsync<T>(string key);
    }
}