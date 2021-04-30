using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
{
    public class MicroItem
    {
        public int Id { get; set; }

        public UserItem User { get; set; }

        public string Content { get; set; }
        [JsonProperty(PropertyName = "extra_rule")]
        public IEnumerable<ExtraRule> ExtraRule { get; set; }

        public IEnumerable<MicroAttachment> Attachment { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }
        [JsonProperty(PropertyName = "open_type")]
        public int OpenType { get; set; }
        [JsonProperty(PropertyName = "recommend_count")]
        public int RecommendCount { get; set; }
        [JsonProperty(PropertyName = "collect_count")]
        public int CollectCount { get; set; }
        [JsonProperty(PropertyName = "forward_count")]
        public int ForwardCount { get; set; }
        [JsonProperty(PropertyName = "comment_count")]
        public int CommentCount { get; set; }
        [JsonProperty(PropertyName = "forward_id")]
        public int ForwardId { get; set; }
        public string Source { get; set; }
        [JsonProperty(PropertyName = "updated_at")]
        public string UpdatedAt { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }
        public bool Editable { get; set; }
        [JsonProperty(PropertyName = "is_recommended")]
        public bool IsRecommended { get; set; }
        [JsonProperty(PropertyName = "is_collected")]
        public bool IsCollected { get; set; }
    }

    public class MicroAttachment
    {
        public int Id { get; set; }
        public string Thumb { get; set; }
        public string File { get; set; }
    }
}
