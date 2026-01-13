using System.Text.Json.Serialization;

namespace ZoDream.LogTimer.Models
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Article
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }
        [JsonPropertyName("comment_count")]
        public int CommentCount { get; set; }

        public int Recommend { get; set; }
        [JsonPropertyName("click_count")]
        public int ClickCount { get; set; }

        public User User { get; set; }

        public ArticleCategory Term { get; set; }

        [JsonPropertyName("video_url")]
        public string VideoUrl { get; set; }
        [JsonPropertyName("audio_url")]
        public string AudioUrl { get; set; }
    }

    /// <summary>
    /// 文章分类
    /// </summary>
    public class ArticleCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Thumb { get; set; }
        [JsonPropertyName("blog_count")]
        public int BlogCount { get; set; }
    }
}
