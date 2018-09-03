using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Cache
{
    public interface IMemoryCache
    {
        void Set(string key, object value);
        void Set(string key, object value, DateTime expiresAt);
        void Set(string key, object value, TimeSpan expiresIn);
        void Set<T>(string key, T value);
        //void Set<T>(string key, T value, DateTime expiresAt);
        //void Set<T>(string key, T value, TimeSpan expiesIn);
        object Get(string key);
        T Get<T>(string key);
        bool Remove(string key);
    }
}
