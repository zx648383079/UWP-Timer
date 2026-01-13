using System.Text.Json.Serialization;

namespace ZoDream.LogTimer.Repositories.Models
{
    /// <summary>
    /// 注销表单
    /// </summary>
    public class CancelForm
    {
        [JsonPropertyName("reason")]
        public string Reason { get; set; }
    }
}
