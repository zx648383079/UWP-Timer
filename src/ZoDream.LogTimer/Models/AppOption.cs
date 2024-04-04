using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Models
{
    public class AppOption
    {
        public string Name { get; set; } = string.Empty;

        public string Version { get; set; } = string.Empty;

        public string Logo { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "site_close")]
        public bool SiteClose { get; set; }

        [JsonProperty(PropertyName = "site_close_tip")]
        public string SiteCloseTip { get; set; } = string.Empty;
    }
}
