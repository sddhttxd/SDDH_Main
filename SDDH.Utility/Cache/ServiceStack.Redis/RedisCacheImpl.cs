using SDDH.Utility.Config;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Cache
{
    public class RedisCacheImpl : IRedisCache
    {
        private RedisCacheImpl() { }
        private static readonly Lazy<RedisCacheImpl> _instance = new Lazy<RedisCacheImpl>(() => { return new RedisCacheImpl(); });

        public static RedisCacheImpl Instance
        {
            get { return _instance.Value; }
        }

        /// <summary>
        /// 连接池管理对象
        /// </summary>
        static Lazy<PooledRedisClientManager> lazyRedisManager = new Lazy<PooledRedisClientManager>(() =>
        {
            int poolSize = ConfigurationAppSetting.RedisPoolSize;
            int poolTimeOutSeconds = ConfigurationAppSetting.RedisPoolTimeOutSeconds;
            string readWriteHosts = ConfigurationAppSetting.RedisServerIP;
            return new PooledRedisClientManager(poolSize, poolTimeOutSeconds, readWriteHosts);
        });
        static PooledRedisClientManager ClientManager
        {
            get { return lazyRedisManager.Value; }
        }

        /// <summary>
        /// 连接池管理对象2
        /// </summary>
        static Lazy<PooledRedisClientManager> lazyRedisManager2 = new Lazy<PooledRedisClientManager>(() =>
        {
            RedisConfigInfo redisConfigInfo = RedisConfigInfo.GetConfig();
            string[] writeServerList = redisConfigInfo.WriteServerList.Split(",".ToArray());//SplitString(redisConfigInfo.WriteServerList, ",");
            string[] readServerList = redisConfigInfo.ReadServerList.Split(",".ToArray()); //SplitString(redisConfigInfo.ReadServerList, ",");
            RedisClientManagerConfig redisClientManagerConfig = new RedisClientManagerConfig
            {
                MaxWritePoolSize = redisConfigInfo.MaxWritePoolSize,
                MaxReadPoolSize = redisConfigInfo.MaxReadPoolSize,
                AutoStart = redisConfigInfo.AutoStart
            };
            return new PooledRedisClientManager(readServerList, writeServerList, redisClientManagerConfig);
        });
        static PooledRedisClientManager ClientManager2
        {
            get { return lazyRedisManager2.Value; }
        }

        public void Set(string key, object value)
        {
            using (IRedisClient client = ClientManager.GetClient())
            {
                client.Set(key, value);
            }
        }

        public void Set(string key, object value, DateTime expiresAt)
        {
            using (IRedisClient client = ClientManager.GetClient())
            {
                client.Set(key, value, expiresAt);
            }
        }

        public void Set(string key, object value, TimeSpan expiresIn)
        {
            using (IRedisClient client = ClientManager.GetClient())
            {
                client.Set(key, value, expiresIn);
            }
        }

        public object Get(string key)
        {
            using (IRedisClient client = ClientManager.GetClient())
            {
                return client.Get<object>(key);
            }
        }

        public T Get<T>(string key)
        {
            using (IRedisClient client = ClientManager.GetClient())
            {
                return client.Get<T>(key);
            }
        }

        public bool Remove(string key)
        {
            using (IRedisClient client = ClientManager.GetClient())
            {
                return client.Remove(key);
            }
        }

        public long Increment(string key)
        {
            using (IRedisClient client = ClientManager.GetClient())
            {
                return client.IncrementValue(key);
            }
        }

        public long Decrement(string key)
        {
            using (IRedisClient client = ClientManager.GetClient())
            {
                return client.DecrementValue(key);
            }
        }

        public void Set<T>(string key, T value)
        {
            using (IRedisClient client = ClientManager.GetClient())
            {
                client.Set<T>(key, value);
            }
        }

        private static string[] SplitString(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }
    }
}
