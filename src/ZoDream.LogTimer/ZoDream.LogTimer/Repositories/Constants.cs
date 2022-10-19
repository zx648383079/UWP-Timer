using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace ZoDream.LogTimer.Repositories
{
    /// <summary>
    /// 全局配置
    /// </summary>
    public static class Constants
    {
        #region API配置
#if _DEBUG
        public const string ApiEndpoint = "<DEV URL>";
        public const string AppId = "<DEV APPID>";
        public const string Secret = "<DEV SECRET>";
#else
        public const string ApiEndpoint = "<PROD URL>";
        public const string AppId = "<PROD APPID>";
        public const string Secret = "<PROD SECRET>";
#endif


        #endregion

        #region 全局键
        /// <summary>
        /// token 保存键
        /// </summary>
        public const string TOKEN_KEY = "token";

        public const string USER_KEY = "user";
        /// <summary>
        /// 设置保存键
        /// </summary>
        public const string SETTING_KEY = "setting";
        #endregion

        #region 语言包获取文字

        private static ResourceLoader CurrentResourceLoader
        {
            get { return _loader ??= new ResourceLoader("Resources"); }//ResourceLoader.GetForCurrentView("Resources"); }
        }

        private static ResourceLoader _loader;
        private static readonly Dictionary<string, string> ResourceCache = new();

        /// <summary>
        /// 获取资源字典的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            if (ResourceCache.TryGetValue(key, out string s))
            {
                return s;
            }
            else
            {
                s = CurrentResourceLoader.GetString(key);
                ResourceCache[key] = s;
                return s;
            }
        }

        #endregion


    }
}
