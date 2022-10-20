using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Models
{
    /// <summary>
    /// 设置表单
    /// </summary>
    public class UserOption
    {
        public bool FullScreen { get; set; } = true;

        public bool Vibrate { get; set; } = true;

        public bool ScreenOn { get; set; } = true;
    }
}
