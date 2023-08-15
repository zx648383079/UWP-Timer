using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public string Avatar { get; set; }

        public string Email { get; set; }

        public int Sex { get; set; }

        [JsonProperty("sex_label")]
        public string SexLabel { get; set; }

        public string Birthday { get; set; }

        public string Token { get; set; }
    }

    public class UserItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Avatar { get; set; }

        public string Icon { get; set; }
    }
}
