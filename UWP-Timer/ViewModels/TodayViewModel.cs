using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;

namespace UWP_Timer.ViewModels
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


        public SettingItem Settings { get; set; }
    }
}
