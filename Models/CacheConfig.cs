namespace EntityCachingLib.Models
{
    public class MemoryCacheConfig
    {
        public bool Enabled { get; set; } = true;
    }

    public class RedisCacheConfig
    {
        public bool Enabled { get; set; } = false;
        public string ConnectionString { get; set; } = string.Empty;
    }

    public class CacheConfig
    {
        public CacheProvider Primary { get; set; } = CacheProvider.Memory;
        public MemoryCacheConfig Memory { get; set; } = new();
        public RedisCacheConfig Redis { get; set; } = new();
    }
}