using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
{
    /// <summary>
    /// 通知
    /// </summary>
    public class Bulletin
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        [JsonProperty(PropertyName = "extra_rule")]
        public IList<ExtraRule> ExtraRule { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty(PropertyName = "updated_at")]
        public string UpdatedAt { get; set; }
        [JsonProperty(PropertyName = "user_name")]
        public string UserName { get; set; }
        public string Icon { get; set; }

        public UserItem User { get; set; }

        public int Status { get; set; }

        internal MessageBase ToMessage()
        {
            return new MessageBase()
            {
                Id = Id,
                User = User,
                Content = $"{Title}\n{Content}",
                ExtraRule = ExtraRule,
                CreatedAt = CreatedAt
            };
        }
    }
    /// <summary>
    /// 通知接收状态
    /// </summary>
    public class BulletinUser
    {
        public int Id { get; set; }
        public int Status { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }
        public Bulletin Bulletin { get; set; }
    }
}
