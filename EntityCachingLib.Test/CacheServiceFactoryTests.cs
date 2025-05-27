using EntityCachingLib.Interfaces;
using EntityCachingLib.Models;
using EntityCachingLib.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;

namespace EntityCachingLib.Test
{
    public class CacheServiceFactoryTests
    {
        [Test]
        public void GetPrimary_ReturnsCorrectProvider()
        {
            var config = new CacheConfig
            {
                Primary = CacheProvider.Memory,
                Memory = new MemoryCacheConfig { Enabled = true },
                Redis = new RedisCacheConfig { Enabled = true, ConnectionString = "localhost" }
            };
            var mem = new MemoryCache(new MemoryCacheOptions());
            var memory = new Mock<MemoryCacheService>(mem).Object;
            var redis = new Mock<RedisCacheService>(Options.Create(config)).Object;
            var factory = new CacheServiceFactory(Options.Create(config), new List<ICacheService> { memory, redis });

            var primary = factory.GetPrimary();
            Assert.AreEqual(memory, primary);
        }

        [Test]
        public void GetAllEnabled_ReturnsEnabledCaches()
        {
            var config = new CacheConfig
            {
                Memory = new MemoryCacheConfig { Enabled = true },
                Redis = new RedisCacheConfig { Enabled = false }
            };
            var mem = new MemoryCache(new MemoryCacheOptions());
            var memory = new Mock<MemoryCacheService>(mem).Object;
            var redis = new Mock<RedisCacheService>(Options.Create(config)).Object;
            var factory = new CacheServiceFactory(Options.Create(config), new List<ICacheService> { memory, redis });

            var enabled = factory.GetAllEnabled();
            CollectionAssert.Contains(enabled, memory);
            CollectionAssert.DoesNotContain(enabled, redis);
        }

        [Test]
        public void GetAllExceptPrimary_ExcludesPrimary()
        {
            var config = new CacheConfig
            {
                Primary = CacheProvider.Memory,
                Memory = new MemoryCacheConfig { Enabled = true },
                Redis = new RedisCacheConfig { Enabled = true, ConnectionString = "localhost" }
            };
            var mem = new MemoryCache(new MemoryCacheOptions());
            var memory = new Mock<MemoryCacheService>(mem).Object;
            var redis = new Mock<RedisCacheService>(Options.Create(config)).Object;
            var factory = new CacheServiceFactory(Options.Create(config), new List<ICacheService> { memory, redis });

            var exceptPrimary = factory.GetAllExceptPrimary();
            CollectionAssert.Contains(exceptPrimary, redis);
            CollectionAssert.DoesNotContain(exceptPrimary, memory);
        }
    }
}