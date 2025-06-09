using EntityCachingLib.Models;
using System.Collections.Generic;

namespace EntityCachingLib.Interfaces
{
    public interface ICacheServiceFactory
    {
        ICacheProvider GetPrimary();
        IEnumerable<ICacheProvider> GetAllEnabled();
        IEnumerable<ICacheProvider> GetAllExceptPrimary();
    }
}