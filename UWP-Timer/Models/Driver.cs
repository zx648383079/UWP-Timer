using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
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
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; } 
    }
}
