using CommunityToolkit.Mvvm.ComponentModel;

namespace ZoDream.LogTimer.Models
{
    /// <summary>
    /// 签到日历天
    /// </summary>
    public class DayItem: ObservableObject
    {
        public int Day { get; set; }

        private bool isChecked = false;

        public bool IsChecked
        {
            get => isChecked;
            set => SetProperty(ref isChecked, value);
        }

    }
}
