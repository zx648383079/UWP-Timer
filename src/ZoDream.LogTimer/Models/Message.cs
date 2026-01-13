using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZoDream.LogTimer.Models
{
    public class MessageBase
    {
        public int Id { get; set; }

        public UserItem User { get; set; }

        public MessageType Type { get; set; } = MessageType.TEXT;

        public string Content { get; set; }

        [JsonPropertyName("extra_rule")]
        public IEnumerable<ExtraRule> ExtraRule { get; set; }

        [JsonPropertyName("created_at")]
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
