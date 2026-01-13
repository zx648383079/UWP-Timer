using System.Text.Json.Serialization;

namespace ZoDream.LogTimer.Repositories.Models
{
    /// <summary>
    /// 注册表单
    /// </summary>
    public class RegisterForm : LoginForm
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("confirm_password")]
        public string RePassword { get; set; }
        [JsonPropertyName("agree")]
        public bool Agree { get; set; } = true;


    }
}
