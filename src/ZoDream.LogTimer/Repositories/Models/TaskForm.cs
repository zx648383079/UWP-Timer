using System.Text.Json.Serialization;

namespace ZoDream.LogTimer.Repositories.Models
{
    /// <summary>
    /// 编辑任务表单
    /// </summary>
    public class TaskForm
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("every_time")]
        public int EveryTime { get; set; }
        [JsonPropertyName("parent_id")]
        public int ParentId { get; set; }
    }

    public class TaskShareForm
    {
        [JsonPropertyName("task_id")]
        public int TaskId { get; set; }
        [JsonPropertyName("share_type")]
        public int ShareType { get; set; }
        [JsonPropertyName("share_rule")]
        public string ShareRule { get; set; }
    }
}
