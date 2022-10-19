using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Models
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
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "thumb")]
        public string Thumb { get; set; }
        [JsonProperty(PropertyName = "file")]
        public string File { get; set; }
    }

    public class MicroForm
    {
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
        [JsonProperty(PropertyName = "open_type")]
        public int OpenType { get; set; }
        [JsonProperty(PropertyName = "file")]
        public IList<MicroAttachment> File { get; set; }
    }

    public class MicroCommentForm
    {
        [JsonProperty(PropertyName = "content")]
        public string Conent { get; set; }
        [JsonProperty(PropertyName = "micro_id")]
        public int MicroId { get; set; }
        [JsonProperty(PropertyName = "parent_id")]
        public int ParentId { get; set; }
        [JsonProperty(PropertyName = "is_forward")]
        public bool IsForward { get; set; }
    }

    public class MicroShareForm
    {
        [JsonProperty(PropertyName = "shareappid")]
        public string Shareappid { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "summary")]
        public string Summary { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "pics")]
        public IList<MicroAttachment> Pics { get; set; }
        [JsonProperty(PropertyName = "sharesource")]
        public string Sharesource { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "open_type")]
        public int OpenType { get; set; }
    }
}
