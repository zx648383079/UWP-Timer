using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
{
    public class Emoji
    {
        public int Id { get; set; }
        [JsonProperty(PropertyName = "cat_id")]
        public int CatId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
    }

    public class EmojiCategory
    {
        public IList<Emoji> Items { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
