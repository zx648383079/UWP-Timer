using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;
using UWP_Timer.Utils;

namespace UWP_Timer.ViewModels
{
    public class MicroShareViewModel : BindableBase
    {

        private string content;

        public string Content
        {
            get => content;
            set => Set(ref content, value);
        }

        private string summary;

        public string Summary
        {
            get => summary;
            set => Set(ref summary, value);
        }


        private string title;

        public string Title
        {
            get => title;
            set => Set(ref title, value);
        }

        private string url;

        public string Url
        {
            get => url;
            set => Set(ref url, value);
        }

        private ShareData source;

        public ShareData Source
        {
            get { return source; }
            set { 
                source = value;
                Title = value.Title;
                Url = value.Url;
                Summary = value.Summary;
                FileItems.Clear();
                foreach (var item in value.Pics)
                {
                    FileItems.Add(new MicroAttachment()
                    {
                        Thumb = item,
                        File = item,
                    });
                }
            }
        }


        private ObservableCollection<MicroAttachment> fileItems = new ObservableCollection<MicroAttachment>();

        public ObservableCollection<MicroAttachment> FileItems
        {
            get => fileItems;
            set => Set(ref fileItems, value);
        }


    }
}
