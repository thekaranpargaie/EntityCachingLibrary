using EntityCachingLib.Interfaces;
using EntityCachingLib.Main.Providers;
using EntityCachingLib.Models;
using EntityCachingLib.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EntityCachingLib.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<CacheConfig>(configuration.GetSection("CacheConfig").Value);
            services.AddSingleton<ICacheProvider, MemoryCacheService>();
            services.AddSingleton<ICacheProvider, RedisCacheService>();
            services.AddSingleton<ICacheServiceFactory, CacheServiceFactory>();
            return services;
        }
    }
}