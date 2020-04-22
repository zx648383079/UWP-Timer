using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;
using UWP_Timer.Repositories.Rest;

namespace UWP_Timer.Repositories
{
    /// <summary>
    /// 通知
    /// </summary>
    public class RestBulletinRepository
    {
        private readonly RestRequest http = new RestRequest();
        /// <summary>
        /// 获取通知列表
        /// </summary>
        /// <param name="form"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<Page<BulletinUser>> GetBulletinListAsync(SearchForm form, Action<HttpException> action = null)
            => await http.GetAsync<Page<BulletinUser>>("auth/bulletin", form.ToQueries(), action);
    }
}
