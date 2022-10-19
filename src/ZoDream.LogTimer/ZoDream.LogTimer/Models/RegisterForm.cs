using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Models
{
    /// <summary>
    /// 注册表单
    /// </summary>
    public class RegisterForm: LoginForm
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("confirm_password")]
        public string RePassword { get; set; }
        [JsonProperty("agree")]
        public bool Agree { get; set; } = true;


    }
}
