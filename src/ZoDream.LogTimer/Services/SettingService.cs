using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Utils;

namespace ZoDream.LogTimer.Services
{
    public class SettingService : ISettingService
    {
        public T Get<T>(string key)
        {
            return AppData.GetValue<T>(key);
        }

        public void Remove(string key)
        {
            AppData.Remove(key);
        }

        public void Set<T>(string key, T value)
        {
            if (value is null)
            {
                Remove(key);
                return;
            }
            AppData.SetValue(key, value);
        }
    }
}
