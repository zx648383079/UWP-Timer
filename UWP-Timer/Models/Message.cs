using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
{
    public class MessageBase
    {
        public int Id { get; set; }

        public UserItem User { get; set; }

        public MessageType Type { get; set; } = MessageType.TEXT;

        public string Content { get; set; }

        [JsonProperty(PropertyName = "extra_rule")]
        public IEnumerable<ExtraRule> ExtraRule { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
    }

    public enum MessageType
    {
        TEXT,
        IMAGE,
        VIDEO,
        VOICE,
        FILE
    }
}
