using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;

namespace UWP_Timer.ViewModels
{
    public class MicroDetailViewModel : BindableBase
    {
        public MicroDetailViewModel()
        {
        }

        private MicroItem data = new MicroItem() { 
            User = new UserItem(),
        };

        public MicroItem Data
        {
            get => data;
            set => Set(ref data, value);
        }

    }
}
