using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Models
{
    /// <summary>
    /// 编辑任务表单
    /// </summary>
    public class TaskForm
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("every_time")]
        public int EveryTime { get; set; }
        [JsonProperty(PropertyName = "parent_id")]
        public int ParentId { get; set; }
    }

    public class TaskShareForm
    {
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
        [JsonProperty("share_type")]
        public int ShareType { get; set; }
        [JsonProperty("share_rule")]
        public string ShareRule { get; set; }
    }
}
