using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Repositories.Models
{
    /// <summary>
    /// 修改密码表单
    /// </summary>
    public class PasswordForm
    {

        [JsonProperty("old_password")]
        public string OldPassword { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("confirm_password")]
        public string RePassword { get; set; }

        public bool VerifyPassword()
        {
            return !string.IsNullOrEmpty(Password) && Password.Length > 5;
        }

        public bool VerifyOldPassword()
        {
            return !string.IsNullOrEmpty(OldPassword) && OldPassword.Length > 5;
        }
    }
}
