using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Models
{
    /// <summary>
    /// 菜单项
    /// </summary>
    public class TabItem
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public TabItem()
        {

        }

        public TabItem(object val, string name)
        {
            Value = val;
            Name = name;
        }
    }
}
