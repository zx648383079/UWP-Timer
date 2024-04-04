namespace ZoDream.LogTimer.Repositories
{
    /// <summary>
    /// 全局配置
    /// </summary>
    public static class Constants
    {
        #region API配置
#if DEBUG
        public const string ApiEndpoint = "<DEV URL>";
        public const string AppId = "<DEV APPID>";
        public const string Secret = "<DEV SECRET>";
        public const string ApiEndpoint = "http://zodream.localhost/open/";
        public const string AppId = "11543906547";
        public const string Secret = "012e936d3d3653b40c6fc5a32e4ea685";
#else
        public const string ApiEndpoint = "<PROD URL>";
        public const string AppId = "<PROD APPID>";
        public const string Secret = "<PROD SECRET>";
        public const string ApiEndpoint = "https://zodream.cn/open/";
        public const string AppId = "11619498261";
        public const string Secret = "b934f7dd69d1902d4e8db1e0cb4d1d04";
#endif


        #endregion

    }
}
