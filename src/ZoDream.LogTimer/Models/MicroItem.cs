using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZoDream.LogTimer.Models
{
    public class MicroItem
    {
        public int Id { get; set; }

        public UserItem User { get; set; }

        public string Content { get; set; }
        [JsonPropertyName("extra_rule")]
        public IEnumerable<ExtraRule> ExtraRule { get; set; }

        public IEnumerable<MicroAttachment> Attachment { get; set; }
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        [JsonPropertyName("open_type")]
        public int OpenType { get; set; }
        [JsonPropertyName("recommend_count")]
        public int RecommendCount { get; set; }
        [JsonPropertyName("collect_count")]
        public int CollectCount { get; set; }
        [JsonPropertyName("forward_count")]
        public int ForwardCount { get; set; }
        [JsonPropertyName("comment_count")]
        public int CommentCount { get; set; }
        [JsonPropertyName("forward_id")]
        public int ForwardId { get; set; }
        public string Source { get; set; }
        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; }
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }
        public bool Editable { get; set; }
        [JsonPropertyName("is_recommended")]
        public bool IsRecommended { get; set; }
        [JsonPropertyName("is_collected")]
        public bool IsCollected { get; set; }
    }

    public class MicroAttachment
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("thumb")]
        public string Thumb { get; set; }
        [JsonPropertyName("file")]
        public string File { get; set; }
    }

    public class MicroForm
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("open_type")]
        public int OpenType { get; set; }
        [JsonPropertyName("file")]
        public IList<MicroAttachment> File { get; set; }
    }

    public class MicroCommentForm
    {
        [JsonPropertyName("content")]
        public string Conent { get; set; }
        [JsonPropertyName("micro_id")]
        public int MicroId { get; set; }
        [JsonPropertyName("parent_id")]
        public int ParentId { get; set; }
        [JsonPropertyName("is_forward")]
        public bool IsForward { get; set; }
    }

    public class MicroShareForm
    {
        [JsonPropertyName("shareappid")]
        public string Shareappid { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("summary")]
        public string Summary { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("pics")]
        public IList<MicroAttachment> Pics { get; set; }
        [JsonPropertyName("sharesource")]
        public string Sharesource { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("open_type")]
        public int OpenType { get; set; }
    }
}
