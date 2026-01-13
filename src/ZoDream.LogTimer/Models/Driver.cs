
using System.Text.Json.Serialization;

namespace ZoDream.LogTimer.Models
{
    /// <summary>
    /// 登录设备
    /// </summary>
    public class Driver
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Region { get; set; }
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; } 
    }
}
