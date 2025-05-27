using EntityCachingLib.Models;
using System.Collections.Generic;

namespace EntityCachingLib.Interfaces
{
    public interface ICacheServiceFactory
    {
        ICacheService GetPrimary();
        IEnumerable<ICacheService> GetAllEnabled();
        IEnumerable<ICacheService> GetAllExceptPrimary();
    }
}