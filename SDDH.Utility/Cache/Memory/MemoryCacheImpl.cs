using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Cache
{
    public class MemoryCacheImpl : IMemoryCache
    {
        private MemoryCacheImpl() { }
        private static readonly Lazy<MemoryCacheImpl> _instance = new Lazy<MemoryCacheImpl>(() => { return new MemoryCacheImpl(); });

        public static MemoryCacheImpl Instance
        {
            get { return _instance.Value; }
        }

        const int defaultTimeOut = 10;//默认过期时间10分钟
        static Lazy<MemoryCache> _lazyMemoryCache = new Lazy<MemoryCache>(() =>
        {
            return MemoryCache.Default;
        });

        static MemoryCache Client
        {
            get { return _lazyMemoryCache.Value; }
        }

        public void Set(string key, object value)
        {
            var cacheItem = new CacheItem(key, value);
            Client.Set(cacheItem, null);
        }

        public void Set(string key, object value, DateTime expiresAt)
        {
            var cacheItem = new CacheItem(key, value);
            var cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = expiresAt;
            Client.Set(cacheItem, cacheItemPolicy);
        }

        public void Set(string key, object value, TimeSpan expiresIn)
        {
            var cacheItem = new CacheItem(key, value);
            var cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.SlidingExpiration = expiresIn;
            Client.Set(cacheItem, cacheItemPolicy);
        }

        public void Set<T>(string key, T value)
        {
            var cacheItem = new CacheItem(key, value);
            Client.Set(cacheItem, null);
        }

        public void Set<T>(string key, T value, DateTime expiresAt)
        {
            var cacheItem = new CacheItem(key, value);
            var cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = expiresAt;
            Client.Set(cacheItem, cacheItemPolicy);
        }

        public void Set<T>(string key, T value, TimeSpan expiresIn)
        {
            var cacheItem = new CacheItem(key, value);
            var cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.SlidingExpiration = expiresIn;
            Client.Set(cacheItem, cacheItemPolicy);
        }

        public object Get(string key)
        {
            return Client.Get(key);
        }
        public T Get<T>(string key)
        {
            return (T)Client.Get(key);
        }

        public bool Remove(string key)
        {
            Client.Remove(key);
            return true;
        }

    }
}
