using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
{
    public class CommentBase
    {
        public int Id { get; set; }

        public UserItem User { get; set; }

        public string Content { get; set; }

        [JsonProperty(PropertyName = "extra_rule")]
        public IEnumerable<ExtraRule> ExtraRule { get; set; }

        [JsonProperty(PropertyName = "agree_count")]
        public int AgreeCount { get; set; }

        [JsonProperty(PropertyName = "disagree_count")]
        public int DisagreeCount { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
    }

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
