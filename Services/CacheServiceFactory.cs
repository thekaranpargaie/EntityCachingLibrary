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
        private readonly IEnumerable<ICacheService> _cacheService;
        public CacheServiceFactory(
            IOptions<CacheConfig> config,
            IEnumerable<ICacheService> cacheService)
        {
            _config = config.Value;
            _cacheService = cacheService;
        }
        public ICacheService GetPrimary()
        {
            return GetService(_config.Primary);
        }
        public IEnumerable<ICacheService> GetAllEnabled()
        {
            if (_config.Memory.Enabled)
                yield return GetService(CacheProvider.Memory);
            if (_config.Redis.Enabled)
                yield return GetService(CacheProvider.Redis);
        }
        public IEnumerable<ICacheService> GetAllExceptPrimary()
        {
            var primary = GetPrimary();
            return GetAllEnabled().Where(s => s != primary);
        }
        private ICacheService GetService(CacheProvider provider)
        {
            return _cacheService.First(s => s.Provider == provider);
        }
    }
}