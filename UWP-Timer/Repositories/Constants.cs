using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace UWP_Timer.Repositories
{
    /// <summary>
    /// 全局配置
    /// </summary>
    public static class Constants
    {
        #region API配置
        public const string ApiEndpoint = "http://zodream.localhost/open/";
        public const string AppId = "<APP ID>";
        public const string Secret = "<APP SECRET>";

        #endregion

        #region 全局键
        /// <summary>
        /// token 保存键
        /// </summary>
        public const string TOKEN_KEY = "token";
        /// <summary>
        /// 设置保存键
        /// </summary>
        public const string SETTING_KEY = "setting";
        #endregion


        #region 全局数据
        public static string Token = string.Empty;
        #endregion

        #region 语言包获取文字

        private static ResourceLoader CurrentResourceLoader
        {
            get { return _loader ?? (_loader = ResourceLoader.GetForCurrentView("Resources")); }
        }

        private static ResourceLoader _loader;
        private static readonly Dictionary<string, string> ResourceCache = new Dictionary<string, string>();

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
