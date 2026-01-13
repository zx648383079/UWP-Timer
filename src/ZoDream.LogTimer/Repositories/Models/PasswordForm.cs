using System.Text.Json.Serialization;

namespace ZoDream.LogTimer.Repositories.Models
{
    /// <summary>
    /// 修改密码表单
    /// </summary>
    public class PasswordForm
    {

        [JsonPropertyName("old_password")]
        public string OldPassword { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("confirm_password")]
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
