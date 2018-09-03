using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Cache
{
    public interface ICacheManager
    {
        IMemoryCache Memory { get; }
        IMemcachedCache Memcached { get; }
        IRedisCache Redis { get; }
    }
}
