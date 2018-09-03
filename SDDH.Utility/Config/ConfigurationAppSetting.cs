using SDDH.Utility.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Config
{
    public class ConfigurationAppSetting
    {
        #region Redis相关配置
        /// <summary>
        /// Redis服务器IP
        /// </summary>
        private static string _redisServerIP;
        public static string RedisServerIP
        {
            get
            {
                if (string.IsNullOrEmpty(_redisServerIP))
                {
                    _redisServerIP = ConfigurationManager.AppSettings["RedisServerIP"];
                }
                return _redisServerIP;
            }
        }

        /// <summary>
        /// Redis连接池，默认30
        /// </summary>
        private static int _redisPoolSize;
        public static int RedisPoolSize
        {
            get
            {
                if (_redisPoolSize <= 0)
                {
                    int temp = 0;
                    int.TryParse(ConfigurationManager.AppSettings["RedisPoolSize"], out temp);
                    _redisPoolSize = temp > 0 ? temp : 30;
                }
                return _redisPoolSize;
            }
        }

        /// <summary>
        /// Redis链接超时时长，默认5
        /// </summary>
        private static int _redispoolTimeOutSeconds;
        public static int RedisPoolTimeOutSeconds
        {
            get
            {
                if (_redispoolTimeOutSeconds <= 0)
                {
                    int temp = 0;
                    int.TryParse(ConfigurationManager.AppSettings["RedisPoolTimeOutSeconds"], out temp);
                    _redispoolTimeOutSeconds = temp > 0 ? temp : 5;
                }
                return _redispoolTimeOutSeconds;
            }
        }
        #endregion

        #region Memcached相关配置
        /// <summary>
        /// Memcached服务器IP
        /// </summary>
        private static IEnumerable<ServerInfo> _memcachedServerIP;
        public static IEnumerable<ServerInfo> MemcachedServerIP
        {
            get
            {
                if (_memcachedServerIP != null)
                {
                    return _memcachedServerIP;
                }
                string memcachedServerIp = ConfigurationManager.AppSettings["MemcachedServerIP"];
                if (!string.IsNullOrEmpty(memcachedServerIp))
                {
                    var serverArr = memcachedServerIp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (serverArr != null)
                    {
                        _memcachedServerIP = serverArr.Select(t =>
                        {
                            var ipArr = t.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            ServerInfo info = new ServerInfo();
                            info.Address = ipArr[0];
                            int port = 0;
                            int.TryParse(ipArr[1], out port);
                            info.Port = port;
                            return info;
                        });
                        return _memcachedServerIP;
                    }
                }
                throw new ConfigurationErrorsException("memcached serverip error");
            }
        }

        /// <summary>
        /// Memcached最小连接池，默认10
        /// </summary>
        private static int _memcachedMinPoolSize;
        public static int MemcachedMinPoolSize
        {
            get
            {
                if (_memcachedMinPoolSize <= 0)
                {
                    int temp = 0;
                    int.TryParse(ConfigurationManager.AppSettings["MemcachedMinPoolSize"], out temp);
                    _memcachedMinPoolSize = temp > 0 ? temp : 10;
                }
                return _memcachedMinPoolSize;
            }
        }

        /// <summary>
        /// Memcached最大连接池，默认100
        /// </summary>
        private static int _memcachedMaxPoolSize;
        public static int MemcachedMaxPoolSize
        {
            get
            {
                if (_memcachedMaxPoolSize <= 0)
                {
                    int temp = 0;
                    int.TryParse(ConfigurationManager.AppSettings["MemcachedMaxPoolSize"], out temp);
                    _memcachedMaxPoolSize = temp > 0 ? temp : 100;
                }
                return _memcachedMaxPoolSize;
            }
        }

        /// <summary>
        /// Memcached连接超时时长，默认10
        /// </summary>
        private static int _memcachedConnectionTimeout;
        public static int MemcachedConnectionTimeout
        {
            get
            {
                if (_memcachedConnectionTimeout <= 0)
                {
                    int temp = 0;
                    int.TryParse(ConfigurationManager.AppSettings["MemcachedConnectionTimeout"], out temp);
                    _memcachedConnectionTimeout = temp > 0 ? temp : 10;
                }
                return _memcachedConnectionTimeout;
            }
        }

        /// <summary>
        /// Memcached请求超时时长，默认30s
        /// </summary>
        private static int _memcachedReceiveTimeout;
        public static int MemcachedReceiveTimeout
        {
            get
            {
                if (_memcachedReceiveTimeout <= 0)
                {
                    int temp = 0;
                    int.TryParse(ConfigurationManager.AppSettings["MemcachedReceiveTimeout"], out temp);
                    _memcachedReceiveTimeout = temp > 0 ? temp : 30;
                }
                return _memcachedReceiveTimeout;
            }
        }

        /// <summary>
        /// Memcached销毁超时时长，默认120s
        /// </summary>
        private static int _memcachedDeadTimeout;
        public static int MemcachedDeadTimeout
        {
            get
            {
                if (_memcachedDeadTimeout <= 0)
                {
                    int temp = 0;
                    int.TryParse(ConfigurationManager.AppSettings["MemcachedDeadTimeout"], out temp);
                    _memcachedDeadTimeout = temp > 0 ? temp : 120;
                }
                return _memcachedDeadTimeout;
            }
        }

        #endregion


    }
}
