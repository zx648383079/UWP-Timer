using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Stores
{
    public delegate void AuthChangedEventHandler();
    public delegate void SystemBootEventHandler();
    internal delegate void TaskChangedEventHandler(TaskStore sender);
    
}
