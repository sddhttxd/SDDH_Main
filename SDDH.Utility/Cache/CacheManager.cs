using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Cache
{
    public class CacheManager : ICacheManager
    {
        private CacheManager() { }

        private static readonly Lazy<CacheManager> _lazyCacheManager = new Lazy<CacheManager>(() => new CacheManager());

        public static ICacheManager Instance
        {
            get { return _lazyCacheManager.Value; }
        }

        public IMemoryCache Memory
        {
            get { return MemoryCacheImpl.Instance; }
        }

        public IMemcachedCache Memcached
        {
            get { return MemcachedCacheImpl.Instance; }
        }

        public IRedisCache Redis
        {
            get { return RedisCacheImpl.Instance; }
        }

    }
}
