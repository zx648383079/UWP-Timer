using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Repositories.Models
{
    /// <summary>
    /// 修改信息表单
    /// </summary>
    public class ProfileForm
    {
        [JsonProperty("sex")]
        public int Sex { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("birthday")]
        public string Birthday { get; set; }
    }
}
