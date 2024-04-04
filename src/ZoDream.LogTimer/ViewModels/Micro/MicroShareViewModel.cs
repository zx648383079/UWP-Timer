using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    public class MicroShareViewModel : ObservableObject
    {
        public MicroShareViewModel()
        {
            SubmitCommand = new RelayCommand(TapSubmit);
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        private RestMicroRepository _api = App.GetService<RestMicroRepository>();
        private INotificationService _notify = App.GetService<INotificationService>();
        private readonly NavigationService _router = App.GetService<NavigationService>();
        private readonly IAuthService _auth = App.GetService<IAuthService>();

        private string content = string.Empty;

        public string Content
        {
            get => content;
            set => SetProperty(ref content, value);
        }

        private string summary = string.Empty;

        public string Summary
        {
            get => summary;
            set => SetProperty(ref summary, value);
        }


        private string title = string.Empty;

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private string url = string.Empty;

        public string Url
        {
            get => url;
            set => SetProperty(ref url, value);
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


        private ObservableCollection<MicroAttachment> fileItems = [];

        public ObservableCollection<MicroAttachment> FileItems
        {
            get => fileItems;
            set => SetProperty(ref fileItems, value);
        }

        public ICommand SubmitCommand { get; private set; }

        private void TapSubmit()
        {
            _ = CreateAsync(new MicroShareForm()
            {
                Content = Content,
                Pics = FileItems,
                Title = Title,
                Url = Url,
                Summary = Summary,
                Shareappid = Source.Appid,
                Sharesource = Source.Sharesource,
            });
        }

        private async Task CreateAsync(MicroShareForm form)
        {
            var data = await _api.ShareSaveAsync(form);
            if (data == null)
            {
                return;
            }
            _app.DispatcherQueue.TryEnqueue(() => {
                _notify.Success("发布成功");
                _router.GoBack();
            });
        }

        public void Load(ShareData? data)
        {
            if (data == null)
            {
                return;
            }
            Source = data;
        }
    }
}
