using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Repositories.Rest;

namespace UWP_Timer.Repositories
{
    public class RestRepository
    {
        public RestRepository()
        {
            client = new RestRequest(new RestInterceptor(Constants.ApiEndpoint, Constants.AppId, Constants.Secret));
        }
        private readonly RestRequest client;
        /// <summary>
        /// 账户相关
        /// </summary>
        public RestAccountRepository Account => new RestAccountRepository(client);
        /// <summary>
        /// 文章
        /// </summary>
        public RestArticleRepository Article => new RestArticleRepository(client);
        /// <summary>
        /// 扫码登录
        /// </summary>
        public RestAuthorizeRepository Authorize => new RestAuthorizeRepository(client);
        /// <summary>
        /// 通知
        /// </summary>
        public RestBulletinRepository Bulletin => new RestBulletinRepository(client);
        /// <summary>
        /// 签到
        /// </summary>
        public RestCheckInRepository CheckIn => new RestCheckInRepository(client);
        /// <summary>
        /// 任务
        /// </summary>
        public RestTaskRepository Task => new RestTaskRepository(client);
        /// <summary>
        /// 用户
        /// </summary>
        public RestUserRepository User => new RestUserRepository(client);

        /// <summary>
        /// 微博客
        /// </summary>
        public RestMicroRepository Micro => new RestMicroRepository(client);

        public RestFileRepository File => new RestFileRepository(client);
        public RestSiteRepository Site => new RestSiteRepository(client);

    }
}
