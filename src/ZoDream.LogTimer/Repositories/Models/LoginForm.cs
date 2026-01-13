using System.Text.Json.Serialization;

namespace ZoDream.LogTimer.Repositories.Models
{
    /// <summary>
    /// 登录表单
    /// </summary>
    public class LoginForm
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("mobile")]
        public string Mobile { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("remember")]
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
