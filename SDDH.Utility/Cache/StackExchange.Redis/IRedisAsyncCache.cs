using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Cache
{
    public interface IRedisAsyncCache : IMemoryCache
    {
        Task<string> GetAsync(string key);
        Task<T> GetAsync<T>(string key);
        Task<bool> RemoveAsync(string key);
    }
}
