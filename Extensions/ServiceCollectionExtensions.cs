using Microsoft.Extensions.DependencyInjection;
using EntityCachingLib.Interfaces;
using EntityCachingLib.Services;
using StackExchange.Redis;
using EntityCachingLib.Models;
using Microsoft.Extensions.Configuration;

namespace EntityCachingLib.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<CacheConfig>(configuration.GetSection("CacheConfig").Value);
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, MemoryCacheService>();
            services.AddSingleton<ICacheService, RedisCacheService>();
            services.AddSingleton<ICacheServiceFactory, CacheServiceFactory>();
            return services;
        }
    }
}