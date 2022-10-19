using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Models
{
    /// <summary>
    /// 登录表单
    /// </summary>
    public class LoginForm
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("mobile")]
        public string Mobile { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("remember")]
        public bool Remember { get; set; }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <returns></returns>
        public bool VerifyEmail()
        {
            return !string.IsNullOrWhiteSpace(Email) && Email.IndexOf('@') > 2 && Email.IndexOf('.') > 2;
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <returns></returns>
        public bool VerifyPassword()
        {
            return !string.IsNullOrEmpty(Password) && Password.Length > 5;
        }
    }
}
