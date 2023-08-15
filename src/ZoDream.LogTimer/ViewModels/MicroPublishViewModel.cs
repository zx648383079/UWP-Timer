using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;

namespace ZoDream.LogTimer.ViewModels
{
    public class MicroPublishViewModel : BindableBase
    {

        private string content;

        public string Content
        {
            get => content;
            set => Set(ref content, value);
        }


        private ObservableCollection<MicroAttachment> fileItems = new ObservableCollection<MicroAttachment>();

        public ObservableCollection<MicroAttachment> FileItems
        {
            get => fileItems;
            set => Set(ref fileItems, value);
        }


    }
}
