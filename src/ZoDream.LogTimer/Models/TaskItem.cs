using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace ZoDream.LogTimer.Models
{
    /// <summary>
    /// 任务
    /// </summary>
    public class TaskItem
    {
        public int Id { get; set; }
        [JsonPropertyName("parent_id")]
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        [JsonPropertyName("every_time")]
        public int EveryTime { get; set; }
        [JsonPropertyName("space_time")]
        public int SpaceTime { get; set; }
        [JsonPropertyName("per_time")]
        public int PerTime { get; set; }

        [JsonPropertyName("time_length")]
        public int TimeLength { get; set; }
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; }
        [JsonPropertyName("start_at")]
        public string StartAt { get; set; }

        [JsonPropertyName("last_at")]
        public int LastAt { get; set; }
    }
    /// <summary>
    /// 任务记录
    /// </summary>
    public class TaskLog
    {
        public int Id { get; set; }
        public int Status { get; set; }
        [JsonPropertyName("outage_time")]
        public int OutageTime { get; set; }
        [JsonPropertyName("end_at")]
        public int EndAt { get; set; }
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }
        public int Time { get; set; }

        public TaskItem Task { get; set; }
        [JsonPropertyName("start_at")]
        public int StartAt { get; set; }
    }
    /// <summary>
    /// 今日任务
    /// </summary>
    public class TaskDay : ObservableObject
    {
        public int Id { get; set; }
        public string Today { get; set; }
        private int amount;

        public int Amount
        {
            get => amount;
            set => SetProperty(ref amount, value);
        }

        [JsonPropertyName("success_amount")]
        public int SuccessAmount { get; set; }
        [JsonPropertyName("pause_amount")]
        public int PauseAmount { get; set; }
        [JsonPropertyName("failure_amount")]
        public int FailureAmount { get; set; }
        public int Status { get; set; }
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; }
        public TaskItem Task { get; set; }
        public TaskLog Log { get; set; }

        public string Tip { get; set; }
    }

    public class TaskShare
    {
        public int Id { get; set; }

        public UserItem User { get; set; }

        public TaskItem Task { get; set; }

        public int ShareType { get; set; }

        public string ShareRule { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }
    }
}
