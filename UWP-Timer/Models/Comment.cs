using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
{
    public class TaskComment
    {
        public int Id { get; set; }

        public UserItem User { get; set; }

        public int Type { get; set; }

        public string Content { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
    }
}
