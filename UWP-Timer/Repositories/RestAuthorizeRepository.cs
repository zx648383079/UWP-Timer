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
    /// 扫码登录
    /// </summary>
    public class RestAuthorizeRepository
    {
        private readonly RestRequest http = new RestRequest();
        /// <summary>
        /// 验证二维码是否有效
        /// </summary>
        /// <param name="token"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<LoginQr> CheckQrTokenAsync(string token, Action<HttpException> action = null)
        {
            return await http.GetAsync<LoginQr>("auth/qr", "token", token, action);
        }
        /// <summary>
        /// 对二维码进行授权
        /// </summary>
        /// <param name="token"></param>
        /// <param name="confirm">允许</param>
        /// <param name="reject">拒绝</param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<LoginQr> AuthorizeQrTokenAsync(string token, bool confirm = false, bool reject = false, Action<HttpException> action = null)
        {
            var data = new Dictionary<string, string>
            {
                { "token", token }
            };
            if (confirm)
            {
                data.Add("confirm", "true");
            }
            else if (reject)
            {
                data.Add("reject", "true");
            }
            return await http.GetAsync<LoginQr>("auth/qr/authorize", data);
        }
    }
}
