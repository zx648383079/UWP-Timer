using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [JsonProperty(PropertyName = "seo_configs")]
        public AppOption SeoConfigs { get; set; }

        [JsonProperty(PropertyName = "auth_profile")]
        public User AuthProfile { get; set; }
    }
}
