using ZoDream.Shared.Http;

namespace ZoDream.LogTimer.Repositories
{
    internal class RestRepository
    {
        public RestRepository()
        {
            Interceptor = new RestStoreInterceptor(Constants.ApiEndpoint, Constants.AppId, Constants.Secret);
            Client = new RestRequest(Interceptor);
        }
        private readonly RestRequest Client;
        private readonly RestInterceptor Interceptor;

        /// <summary>
        /// 账户相关
        /// </summary>
        public RestAccountRepository Account => new(Client);
        /// <summary>
        /// 文章
        /// </summary>
        public RestArticleRepository Article => new(Client);
        /// <summary>
        /// 扫码登录
        /// </summary>
        public RestAuthorizeRepository Authorize => new(Client);
        /// <summary>
        /// 通知
        /// </summary>
        public RestBulletinRepository Bulletin => new(Client);
        /// <summary>
        /// 签到
        /// </summary>
        public RestCheckInRepository CheckIn => new(Client);
        /// <summary>
        /// 任务
        /// </summary>
        public RestTaskRepository Task => new(Client);
        /// <summary>
        /// 用户
        /// </summary>
        public RestUserRepository User => new(Client);

        /// <summary>
        /// 微博客
        /// </summary>
        public RestMicroRepository Micro => new(Client);

        public RestFileRepository File => new(Client);
        public RestSiteRepository Site => new(Client);
    }
}
