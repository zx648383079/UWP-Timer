using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
{
    /// <summary>
    /// 第三方账号关联信息
    /// </summary>
    public class Connect
    {
        public int Id { get; set; }

        public string Icon { get; set; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public string Label { get; set; }
    }
}
