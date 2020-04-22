using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Repositories
{
    public class RestRepository
    {
        /// <summary>
        /// 账户相关
        /// </summary>
        public RestAccountRepository Account => new RestAccountRepository();
        /// <summary>
        /// 文章
        /// </summary>
        public RestArticleRepository Article => new RestArticleRepository();
        /// <summary>
        /// 扫码登录
        /// </summary>
        public RestAuthorizeRepository Authorize => new RestAuthorizeRepository();
        /// <summary>
        /// 通知
        /// </summary>
        public RestBulletinRepository Bulletin => new RestBulletinRepository();
        /// <summary>
        /// 签到
        /// </summary>
        public RestCheckInRepository CheckIn => new RestCheckInRepository();
        /// <summary>
        /// 任务
        /// </summary>
        public RestTaskRepository Task => new RestTaskRepository();
        /// <summary>
        /// 用户
        /// </summary>
        public RestUserRepository User => new RestUserRepository();

    }
}
