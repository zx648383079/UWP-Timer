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
    /// 账户相关
    /// </summary>
    public class RestAccountRepository
    {
        public RestAccountRepository(RestRequest client)
        {
            http = client;
        }
        private readonly RestRequest http;
        /// <summary>
        /// 获取账户关联列表
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<ResponseData<Connect>> GetConnectAsync(Action<HttpException> action = null)
        {
            return await http.GetAsync<ResponseData<Connect>>("auth/account/connect", action);
        }
        /// <summary>
        /// 获取登录设备
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<ResponseData<Driver>> GetDriverAsync(Action<HttpException> action = null)
        {
            return await http.GetAsync<ResponseData<Driver>>("auth/account/driver", action);
        }
        /// <summary>
        /// 保存反馈
        /// </summary>
        /// <param name="feedback"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<ResponseDataOne<bool>> SaveFeedbackAsync(Feedback feedback, HttpExceptionFunc action = null)
        {
            return await http.PostAsync<Feedback, ResponseDataOne<bool>>("contact/home/feedback", feedback, action);
        }
        /// <summary>
        /// 注销账户
        /// </summary>
        /// <param name="form"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<User> CancelUserAsync(CancelForm form, HttpExceptionFunc action = null)
        {
            return await http.PostAsync<CancelForm, User>("auth/account/cancel", form, action);
        }
    }
}
