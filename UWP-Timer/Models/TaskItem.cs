using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
{
    /// <summary>
    /// 任务
    /// </summary>
    public class TaskItem
    {
        public int Id { get; set; }
        [JsonProperty(PropertyName = "parent_id")]
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        [JsonProperty(PropertyName = "every_time")]
        public int EveryTime { get; set; }
        [JsonProperty(PropertyName = "time_length")]
        public int TimeLength { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty(PropertyName = "updated_at")]
        public string UpdatedAt { get; set; }
        [JsonProperty(PropertyName = "start_at")]
        public int StartAt { get; set; }

        [JsonProperty(PropertyName = "last_at")]
        public int LastAt { get; set; }
    }
    /// <summary>
    /// 任务记录
    /// </summary>
    public class TaskLog
    {
        public int Id { get; set; }
        public int Status { get; set; }
        [JsonProperty(PropertyName = "outage_time")]
        public int OutageTime { get; set; }
        [JsonProperty(PropertyName = "end_at")]
        public int EndAt { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }
        public int Time { get; set; }

        public TaskItem Task { get; set; }
        [JsonProperty(PropertyName = "start_at")]
        public int StartAt { get; set; }
    }
    /// <summary>
    /// 今日任务
    /// </summary>
    public class TaskDay
    {
        public int Id { get; set; }
        public string Today { get; set; }
        public int Amount { get; set; }
        [JsonProperty(PropertyName = "success_amount")]
        public int SuccessAmount { get; set; }
        [JsonProperty(PropertyName = "pause_amount")]
        public int PauseAmount { get; set; }
        [JsonProperty(PropertyName = "failure_amount")]
        public int FailureAmount { get; set; }
        public int Status { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty(PropertyName = "updated_at")]
        public string UpdatedAt { get; set; }
        public TaskItem Task { get; set; }
        public TaskLog Log { get; set; }

        public string Tip { get; set; }
    }
}
