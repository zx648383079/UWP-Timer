using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
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
        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }

        public int Recommend { get; set; }
        [JsonProperty("click_count")]
        public int ClickCount { get; set; }

        public User User { get; set; }

        public ArticleCategory Term { get; set; }
    }

    /// <summary>
    /// 文章分类
    /// </summary>
    public class ArticleCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Thumb { get; set; }
        [JsonProperty("blog_count")]
        public int BlogCount { get; set; }
    }
}
