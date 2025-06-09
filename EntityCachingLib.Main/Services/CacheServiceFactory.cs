using EntityCachingLib.Interfaces;
using EntityCachingLib.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace EntityCachingLib.Services
{
    public class CacheServiceFactory : ICacheServiceFactory
    {
        private readonly CacheConfig _config;
        private readonly IEnumerable<ICacheProvider> _cacheService;
        public CacheServiceFactory(
            IOptions<CacheConfig> config,
            IEnumerable<ICacheProvider> cacheService)
        {
            _config = config.Value;
            _cacheService = cacheService;
        }
        public ICacheProvider GetPrimary()
        {
            return GetService(_config.Primary);
        }
        public IEnumerable<ICacheProvider> GetAllEnabled()
        {
            if (_config.Memory.Enabled)
                yield return GetService(CacheProvider.Memory);
            if (_config.Redis.Enabled)
                yield return GetService(CacheProvider.Redis);
        }
        public IEnumerable<ICacheProvider> GetAllExceptPrimary()
        {
            var primary = GetPrimary();
            return GetAllEnabled().Where(s => s != primary);
        }
        private ICacheProvider GetService(CacheProvider provider)
        {
            return _cacheService.First(s => s.Provider == provider);
        }
    }
}