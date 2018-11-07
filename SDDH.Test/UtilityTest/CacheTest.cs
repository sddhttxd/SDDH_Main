using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using SDDH.Utility.Cache;

namespace SDDH.Test
{
    [TestClass]
    public class CacheTest
    {
        [TestMethod]
        public void MemoryCacheTest()
        {
            try
            {
                string key = "test_Key";
                string value = "test_Value";
                CacheManager.Instance.Memory.Set(key, value);
                var result = CacheManager.Instance.Memory.Get(key);
                CacheManager.Instance.Memory.Set(key, value + "1", DateTime.Now.AddSeconds(5));
                var result1 = CacheManager.Instance.Memory.Get(key);
                CacheManager.Instance.Memory.Set(key, value + "2", TimeSpan.FromSeconds(5));
                var result2 = CacheManager.Instance.Memory.Get(key);
                User obj = new User { Age = 23, Name = "zhangsan" };
                CacheManager.Instance.Memory.Set<User>(key, obj);
                var result3 = CacheManager.Instance.Memory.Get<User>(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [TestMethod]
        public void MemcachedCacheTest()
        {
            try
            {
                string key = "test_Key";
                string value = "test_Value";
                CacheManager.Instance.Memcached.Set(key, value);
                var result = CacheManager.Instance.Memcached.Get(key);
                CacheManager.Instance.Memcached.Set(key, value + "1", DateTime.Now.AddSeconds(5));
                var result1 = CacheManager.Instance.Memcached.Get(key);
                CacheManager.Instance.Memcached.Set(key, value + "2", TimeSpan.FromSeconds(5));
                var result2 = CacheManager.Instance.Memcached.Get(key);
                User obj = new User { Age = 23, Name = "zhangsan" };
                CacheManager.Instance.Memcached.Set<User>(key, obj);
                var result3 = CacheManager.Instance.Memcached.Get<User>(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void RedisCacheTest()
        {
            try
            {
                string key = "test_Key";
                string value = "test_Value";
                CacheManager.Instance.Redis.Set(key, value);
                var result = CacheManager.Instance.Redis.Get(key);
                CacheManager.Instance.Redis.Set(key, value + "1", DateTime.Now.AddSeconds(5));
                var result1 = CacheManager.Instance.Redis.Get(key);
                CacheManager.Instance.Redis.Set(key, value + "2", TimeSpan.FromSeconds(5));
                var result2 = CacheManager.Instance.Redis.Get(key);
                User obj = new User { Age = 23, Name = "zhangsan" };
                CacheManager.Instance.Redis.Set<User>(key, obj);
                var result3 = CacheManager.Instance.Redis.Get<User>(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void RedisAsyncTest()
        {
            try
            {
                string key = "testKey";
                string value = "RedisAsyncTest";
                CacheManager.Instance.RedisAsync.Set(key, value);
                var result = CacheManager.Instance.RedisAsync.Get(key);
            }
            catch (Exception ex)
            {
            }
        }

    }


    public class User
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }
}
