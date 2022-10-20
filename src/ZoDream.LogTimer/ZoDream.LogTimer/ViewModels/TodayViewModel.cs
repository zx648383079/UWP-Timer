using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;

namespace ZoDream.LogTimer.ViewModels
{
    public class TodayViewModel: BindableBase
    {

        private TaskDay today;

        public TaskDay Today
        {
            get => today;
            set => Set(ref today, value);
        }

        private bool isRunning = false;

        public bool IsRunning
        {
            get => isRunning;
            set => Set(ref isRunning, value);
        }


        public UserOption Settings { get; set; }
    }
}
