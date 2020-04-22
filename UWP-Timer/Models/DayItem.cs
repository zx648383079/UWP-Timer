using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
{
    /// <summary>
    /// 签到日历天
    /// </summary>
    public class DayItem: ViewModels.BindableBase
    {
        public int Day { get; set; }

        private bool isChecked = false;

        public bool IsChecked
        {
            get => isChecked;
            set => Set(ref isChecked, value);
        }

    }
}
