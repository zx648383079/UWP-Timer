using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZoDream.LogTimer.Models
{
    public class CommentBase
    {
        public int Id { get; set; }

        public UserItem User { get; set; }

        public string Content { get; set; }

        [JsonPropertyName("extra_rule")]
        public IEnumerable<ExtraRule> ExtraRule { get; set; }

        [JsonPropertyName("agree_count")]
        public int AgreeCount { get; set; }

        [JsonPropertyName("disagree_count")]
        public int DisagreeCount { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("replies")]
        public IList<CommentBase> Children { get; set; }
    }

    public class TaskComment
    {
        public int Id { get; set; }

        public UserItem User { get; set; }

        public int Type { get; set; }

        public string Content { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }
    }
}
