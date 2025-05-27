using System;
using System.Threading.Tasks;

namespace EntityCachingLib.Interfaces
{
    public interface IEntityCacheManager<TEntity, TKey> where TEntity : class
    {
        Task<TEntity?> GetAsync(TKey id, Func<TKey, Task<TEntity?>> fetchFromDb);
        Task SetAsync(TKey id, TEntity entity);
        Task RemoveAsync<TEntity>(TKey id);
    }
}