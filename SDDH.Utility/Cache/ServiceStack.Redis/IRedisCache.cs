using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Cache
{
    public interface IRedisCache : IMemoryCache
    {
        long Increment(string key);
        long Decrement(string key);
    }
}
