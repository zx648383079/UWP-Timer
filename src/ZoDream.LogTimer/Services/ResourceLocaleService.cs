using Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;

namespace ZoDream.LogTimer.Services
{
    public class ResourceLocaleService : ILocaleService
    {
        private ResourceLoader CurrentResourceLoader {
            get { return _loader ??= new ResourceLoader("Resources"); }//ResourceLoader.GetForCurrentView("Resources"); }
        }

        private ResourceLoader? _loader;
        private readonly Dictionary<string, string> ResourceCache = [];

        /// <summary>
        /// 获取资源字典的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            if (ResourceCache.TryGetValue(key, out string? s))
            {
                return s;
            }
            else
            {
                // var data = new ResourceManager().MainResourceMap.GetValue("ZoDream.LogTimer/Strings/Resources");
                s = CurrentResourceLoader.GetString(key);
                ResourceCache[key] = s;
                return s;
            }
        }
    }
}
