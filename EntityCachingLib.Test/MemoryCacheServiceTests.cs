using EntityCachingLib.Services;
using Microsoft.Extensions.Caching.Memory;

namespace EntityCachingLib.Test
{
    public class MemoryCacheServiceTests
    {
        [Test]
        public async Task SetAndGetAsync_Works()
        {
            var mem = new MemoryCache(new MemoryCacheOptions());
            var service = new MemoryCacheService(mem);

            await service.SetAsync<string>("key", "value");
            var result = await service.GetAsync<string>("key");

            Assert.AreEqual("value", result);
        }

        [Test]
        public async Task RemoveAsync_RemovesItem()
        {
            var mem = new MemoryCache(new MemoryCacheOptions());
            var service = new MemoryCacheService(mem);

            await service.SetAsync<string>("key", "value");
            await service.RemoveAsync<string>("key");
            var result = await service.GetAsync<string>("key");

            Assert.IsNull(result);
        }
    }
}