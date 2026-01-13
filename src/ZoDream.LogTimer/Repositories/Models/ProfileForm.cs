using System.Text.Json.Serialization;

namespace ZoDream.LogTimer.Repositories.Models
{
    /// <summary>
    /// 修改信息表单
    /// </summary>
    public class ProfileForm
    {
        [JsonPropertyName("sex")]
        public int Sex { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("birthday")]
        public string Birthday { get; set; }
    }
}
