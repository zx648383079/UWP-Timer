using System.Text.Json.Serialization;

namespace ZoDream.LogTimer.Models
{
    public class AppOption
    {
        public string Name { get; set; } = string.Empty;

        public string Version { get; set; } = string.Empty;

        public string Logo { get; set; } = string.Empty;

        [JsonPropertyName("site_close")]
        public bool SiteClose { get; set; }

        [JsonPropertyName("site_close_tip")]
        public string SiteCloseTip { get; set; } = string.Empty;
    }
}
