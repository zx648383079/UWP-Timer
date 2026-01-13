using System.Text.Json.Serialization;

namespace ZoDream.LogTimer.Models
{
    /// <summary>
    /// 签到信息
    /// </summary>
    public class CheckIn
    {
        public int Type { get; set; }
        public int Method { get; set; }
        public int Running { get; set; }
        public int Money { get; set; }
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }
        public int Id { get; set; }
    }
}
