using System.Collections.Generic;
using System.Text.Json.Serialization;
using ZoDream.LogTimer.Models;

namespace ZoDream.LogTimer.Repositories.Models
{
    public class CheckInBatch
    {
        public CheckIn Today { get; set; }
        public IList<CheckIn> Month { get; set; }
    }

    public class BatchData
    {
        [JsonPropertyName("seo_configs")]
        public AppOption SeoConfigs { get; set; }

        [JsonPropertyName("auth_profile")]
        public User AuthProfile { get; set; }
    }
}
