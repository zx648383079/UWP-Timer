using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
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
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        public int Id { get; set; }
    }
}
