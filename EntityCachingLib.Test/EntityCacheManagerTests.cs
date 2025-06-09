using EntityCachingLib.Interfaces;
using EntityCachingLib.Services;
using Moq;

namespace EntityCachingLib.Test
{
    public class EntityCacheManagerTests
    {
        [Test]
        public async Task GetAsync_ReturnsFromPrimaryCache_IfExists()
        {
            var mockFactory = new Mock<ICacheServiceFactory>();
            var mockPrimary = new Mock<ICacheProvider>();
            mockPrimary.Setup(x => x.GetAsync<string>("Test:1")).ReturnsAsync("cached");
            mockFactory.Setup(x => x.GetPrimary()).Returns(mockPrimary.Object);
            mockFactory.Setup(x => x.GetAllExceptPrimary()).Returns(new List<ICacheProvider>());

            var manager = new EntityCacheManager<string, int>(mockFactory.Object, "Test");
            var result = await manager.GetAsync(1, _ => Task.FromResult<string?>(null));

            Assert.AreEqual("cached", result);
        }

        [Test]
        public async Task GetAsync_FetchesFromOtherCache_IfNotInPrimary()
        {
            var mockFactory = new Mock<ICacheServiceFactory>();
            var mockPrimary = new Mock<ICacheProvider>();
            var mockSecondary = new Mock<ICacheProvider>();
            mockPrimary.Setup(x => x.GetAsync<string>("Test:2")).ReturnsAsync((string?)null);
            mockSecondary.Setup(x => x.GetAsync<string>("Test:2")).ReturnsAsync("secondary");
            mockFactory.Setup(x => x.GetPrimary()).Returns(mockPrimary.Object);
            mockFactory.Setup(x => x.GetAllExceptPrimary()).Returns(new List<ICacheProvider> { mockSecondary.Object });

            var manager = new EntityCacheManager<string, int>(mockFactory.Object, "Test");
            var result = await manager.GetAsync(2, _ => Task.FromResult<string?>(null));

            Assert.AreEqual("secondary", result);
            mockPrimary.Verify(x => x.SetAsync("Test:2", "secondary", It.IsAny<System.TimeSpan>()), Times.Once);
        }

        [Test]
        public async Task GetAsync_FetchesFromDb_IfNotInAnyCache()
        {
            var mockFactory = new Mock<ICacheServiceFactory>();
            var mockPrimary = new Mock<ICacheProvider>();
            mockPrimary.Setup(x => x.GetAsync<string>("Test:3")).ReturnsAsync((string?)null);
            mockFactory.Setup(x => x.GetPrimary()).Returns(mockPrimary.Object);
            mockFactory.Setup(x => x.GetAllExceptPrimary()).Returns(new List<ICacheProvider>());
            mockFactory.Setup(x => x.GetAllEnabled()).Returns(new List<ICacheProvider> { mockPrimary.Object });

            var manager = new EntityCacheManager<string, int>(mockFactory.Object, "Test");
            var result = await manager.GetAsync(3, _ => Task.FromResult<string?>("fromdb"));

            Assert.AreEqual("fromdb", result);
            mockPrimary.Verify(x => x.SetAsync("Test:3", "fromdb", It.IsAny<System.TimeSpan>()), Times.Once);
        }

        [Test]
        public async Task SetAsync_SetsAllEnabledCaches()
        {
            var mockFactory = new Mock<ICacheServiceFactory>();
            var mockCache1 = new Mock<ICacheProvider>();
            var mockCache2 = new Mock<ICacheProvider>();
            mockFactory.Setup(x => x.GetAllEnabled()).Returns(new List<ICacheProvider> { mockCache1.Object, mockCache2.Object });

            var manager = new EntityCacheManager<string, int>(mockFactory.Object, "Test");
            await manager.SetAsync(4, "val");

            mockCache1.Verify(x => x.SetAsync("Test:4", "val", It.IsAny<System.TimeSpan>()), Times.Once);
            mockCache2.Verify(x => x.SetAsync("Test:4", "val", It.IsAny<System.TimeSpan>()), Times.Once);
        }

        [Test]
        public async Task RemoveAsync_RemovesFromAllEnabledCaches()
        {
            var mockFactory = new Mock<ICacheServiceFactory>();
            var mockCache1 = new Mock<ICacheProvider>();
            var mockCache2 = new Mock<ICacheProvider>();
            mockFactory.Setup(x => x.GetAllEnabled()).Returns(new List<ICacheProvider> { mockCache1.Object, mockCache2.Object });

            var manager = new EntityCacheManager<string, int>(mockFactory.Object, "Test");
            await manager.RemoveAsync<string>(5);

            mockCache1.Verify(x => x.RemoveAsync<string>("Test:5"), Times.Once);
            mockCache2.Verify(x => x.RemoveAsync<string>("Test:5"), Times.Once);
        }
    }
}