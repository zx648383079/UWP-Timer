using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
{
    /// <summary>
    /// 设置表单
    /// </summary>
    public class SettingItem
    {
        public bool FullScreen { get; set; } = true;

        public bool Vibrate { get; set; } = true;

        public bool ScreenOn { get; set; } = true;
    }
}
