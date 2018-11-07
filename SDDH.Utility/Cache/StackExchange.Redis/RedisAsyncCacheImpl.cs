using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Cache.StackExchange.Redis
{
    public class RedisAsyncCacheImpl : IRedisAsyncCache
    {
        public static readonly Lazy<RedisAsyncCacheImpl> _instance = new Lazy<RedisAsyncCacheImpl>(() => { return new RedisAsyncCacheImpl(); });

        //private static readonly string Coonstr = ConfigurationManager.ConnectionStrings["RedisServerIP"].ConnectionString;
        private static readonly string Coonstr = ConfigurationManager.AppSettings["RedisServerIP"];
        private static object _locker = new Object();
        private static ConnectionMultiplexer _manager = null;
        private static IDatabase _db = null;

        public RedisAsyncCacheImpl()
        {
            _db = RedisManager.GetDatabase();
        }

        public static RedisAsyncCacheImpl Instance
        {
            get { return _instance.Value; }
        }

        /// <summary>
        /// 使用一个静态属性来返回已连接的实例，如下列中所示。这样，一旦 ConnectionMultiplexer 断开连接，便可以初始化新的连接实例。
        /// </summary>
        public static ConnectionMultiplexer RedisManager
        {
            get
            {
                if (_manager == null)
                {
                    lock (_locker)
                    {
                        if (_manager == null || !_manager.IsConnected)
                        {
                            _manager = ConnectionMultiplexer.Connect(Coonstr);
                        }
                    }
                }
                //注册如下事件
                _manager.ConnectionFailed += MuxerConnectionFailed;
                _manager.ConnectionRestored += MuxerConnectionRestored;
                _manager.ErrorMessage += MuxerErrorMessage;
                _manager.ConfigurationChanged += MuxerConfigurationChanged;
                _manager.HashSlotMoved += MuxerHashSlotMoved;
                _manager.InternalError += MuxerInternalError;
                return _manager;
            }
        }

        public object Get(string key)
        {
            return _db.StringGet(key);
        }

        public async Task<string> GetAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public T Get<T>(string key)
        {
            return JsonConvert.DeserializeObject<T>(_db.StringGet(key));
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var result = await _db.StringGetAsync(key);
            return JsonConvert.DeserializeObject<T>(result);
        }

        public bool Remove(string key)
        {
            return _db.KeyDelete(key);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }

        public void Set(string key, object value)
        {
            _db.StringSet(key, JsonConvert.SerializeObject(value));
        }

        public void SetAsync(string key, object value)
        {
            _db.StringSetAsync(key, JsonConvert.SerializeObject(value));
        }

        public void Set(string key, object value, TimeSpan expiresIn)
        {
            _db.StringSet(key, JsonConvert.SerializeObject(value), expiresIn);
        }

        public void SetAsync(string key, object value, TimeSpan expiresIn)
        {
            _db.StringSetAsync(key, JsonConvert.SerializeObject(value), expiresIn);
        }

        public void Set(string key, object value, DateTime expiresAt)
        {
            _db.StringSet(key, JsonConvert.SerializeObject(value), expiresAt - DateTime.Now);
        }

        public void SetAsync(string key, object value, DateTime expiresAt)
        {
            _db.StringSetAsync(key, JsonConvert.SerializeObject(value), expiresAt - DateTime.Now);
        }

        public void Set<T>(string key, T value)
        {
            _db.StringSet(key, JsonConvert.SerializeObject(value));
        }
        public void SetAsync<T>(string key, T value)
        {
            _db.StringSetAsync(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            //LogHelper.WriteInfoLog("Configuration changed: " + e.EndPoint);
        }
        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            //LogHelper.WriteInfoLog("ErrorMessage: " + e.Message);
        }
        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            //LogHelper.WriteInfoLog("ConnectionRestored: " + e.EndPoint);
        }
        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            //LogHelper.WriteInfoLog("重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
        }
        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            //LogHelper.WriteInfoLog("HashSlotMoved:NewEndPoint" + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
        }
        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            //LogHelper.WriteInfoLog("InternalError:Message" + e.Exception.Message);
        }

    }
}
