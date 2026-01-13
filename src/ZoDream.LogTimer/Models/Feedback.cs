using System.Text.Json.Serialization;

namespace ZoDream.LogTimer.Models
{
    /// <summary>
    /// 反馈表单
    /// </summary>
    public class Feedback
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
