using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Services
{
    public interface ICacheService
    {
        public Task<T?> GetOrSetAsync<T>(string name, Func<Task<T?>> func);

        public Task<T?> GetAsync<T>(string name);
        public Task RemoveAsync<T>(string name);

        public Task SetAsync<T>(string name, T data);

        public Task ClearAsync();
    }
}
