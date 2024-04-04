using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Services
{
    public interface ISettingService
    {
        public void Set<T>(string key, T value);
        public T Get<T>(string key);
        public void Remove(string key);
    }
}
