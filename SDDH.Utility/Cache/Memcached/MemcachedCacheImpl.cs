using Enyim.Caching.Configuration;
using SDDH.Utility.Config;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Cache
{
    public class MemcachedCacheImpl : IMemcachedCache
    {
        private MemcachedCacheImpl() { }
        private static readonly Lazy<MemcachedCacheImpl> _instance = new Lazy<MemcachedCacheImpl>(() => { return new MemcachedCacheImpl(); });

        public static MemcachedCacheImpl Instance
        {
            get { return _instance.Value; }
        }

        static Lazy<ICacheClient> _lazyMemcachedClient = new Lazy<ICacheClient>(() =>
        {
            var memcachedClientConfiguration = new MemcachedClientConfiguration();
            var memcachedServerIpList = ConfigurationAppSetting.MemcachedServerIP;
            foreach (var server in memcachedServerIpList)
            {
                memcachedClientConfiguration.AddServer(server.Address, server.Port);
            }
            memcachedClientConfiguration.SocketPool.MinPoolSize = ConfigurationAppSetting.MemcachedMinPoolSize;
            memcachedClientConfiguration.SocketPool.MaxPoolSize = ConfigurationAppSetting.MemcachedMaxPoolSize;
            memcachedClientConfiguration.SocketPool.ConnectionTimeout = TimeSpan.FromSeconds(ConfigurationAppSetting.MemcachedConnectionTimeout);
            memcachedClientConfiguration.SocketPool.ReceiveTimeout = TimeSpan.FromSeconds(ConfigurationAppSetting.MemcachedReceiveTimeout);
            memcachedClientConfiguration.SocketPool.DeadTimeout = TimeSpan.FromSeconds(ConfigurationAppSetting.MemcachedDeadTimeout);//new TimeSpan(0, 2, 0);
            return new MemcachedClientCache(memcachedClientConfiguration);
        });


        static ICacheClient Client
        {
            get { return _lazyMemcachedClient.Value; }
        }

        public void Set(string key, object value)
        {
            Client.Set(key, value);
        }

        public void Set(string key, object value, DateTime expiresAt)
        {
            Client.Set(key, value, expiresAt);
        }

        public void Set(string key, object value, TimeSpan expiresIn)
        {
            Client.Set(key, value, expiresIn);
        }

        public object Get(string key)
        {
            return Client.Get<object>(key);
        }

        public T Get<T>(string key)
        {
            return Client.Get<T>(key);
        }

        public bool Remove(string key)
        {
            return Client.Remove(key);
        }

        public long Increment(string key)
        {
            return Client.Increment(key, 1);
        }

        public long Decrement(string key)
        {
            return Client.Decrement(key, 1);
        }


        public void Set<T>(string key, T value)
        {
            Client.Set<T>(key, value);
        }

    }
}
