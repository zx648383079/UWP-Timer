using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories.Models;
using ZoDream.Shared.Http;

namespace ZoDream.LogTimer.Repositories
{
    /// <summary>
    /// 通知
    /// </summary>
    public class RestBulletinRepository(RestRequest client)
    {
        private readonly RestRequest http = client;
        /// <summary>
        /// 获取通知列表
        /// </summary>
        /// <param name="form"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<Page<BulletinUser>> GetBulletinListAsync(SearchForm form, HttpExceptionFunc action = null)
            => await http.GetAsync<Page<BulletinUser>>("auth/bulletin", form.ToQueries(), action);

        public async Task<ResponseData<UserItem>> GetUserListAsync(HttpExceptionFunc action = null)
            => await http.GetAsync<ResponseData<UserItem>>("auth/bulletin/user", action);
    }
}
