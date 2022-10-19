using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.Shared.Http;

namespace ZoDream.LogTimer.Repositories
{
    /// <summary>
    /// 扫码登录
    /// </summary>
    public class RestAuthorizeRepository
    {
        public RestAuthorizeRepository(RestRequest client)
        {
            http = client;
        }
        private readonly RestRequest http;
        /// <summary>
        /// 验证二维码是否有效
        /// </summary>
        /// <param name="token"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<LoginQr> CheckQrTokenAsync(string token, HttpExceptionFunc action = null)
        {
            return await http.PostAsync<LoginQr>("auth/qr", new Dictionary<string, string>() {
                { "token", token } 
            }, action);
        }
        /// <summary>
        /// 对二维码进行授权
        /// </summary>
        /// <param name="token"></param>
        /// <param name="confirm">允许</param>
        /// <param name="reject">拒绝</param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<LoginQr> AuthorizeQrTokenAsync(string token, bool confirm = false, bool reject = false, HttpExceptionFunc action = null)
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
            return await http.PostAsync<LoginQr>("auth/qr/authorize", data, action);
        }

        public async Task<QrData> QrRefreshAsync(HttpExceptionFunc action = null)
        {
            return await http.GetAsync<QrData>("auth/qr/refresh", action);
        }

        public async Task<User> QrCheckAsync(string token, HttpExceptionFunc action = null)
        {
            var data = new Dictionary<string, string>
            {
                { "token", token }
            };
            return await http.PostAsync<User>("auth/qr/check", data, action);
        }
    }
}
