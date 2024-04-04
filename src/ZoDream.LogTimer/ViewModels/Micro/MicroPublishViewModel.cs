using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Services;
using ZoDream.LogTimer.Utils;

namespace ZoDream.LogTimer.ViewModels
{
    public class MicroPublishViewModel : ObservableObject
    {
        public MicroPublishViewModel()
        {
            SubmitCommand = new RelayCommand(TapSubmit);
            PickCommand = new RelayCommand(TapPick);
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        private RestMicroRepository _api = App.GetService<RestMicroRepository>();
        private RestFileRepository _upload = App.GetService<RestFileRepository>();
        private INotificationService _notify = App.GetService<INotificationService>();
        private readonly NavigationService _router = App.GetService<NavigationService>();
        private readonly IAuthService _auth = App.GetService<IAuthService>();

        private string content = string.Empty;

        public string Content
        {
            get => content;
            set => SetProperty(ref content, value);
        }

        private int openType;

        public int OpenType {
            get => openType;
            set => SetProperty(ref openType, value);
        }


        private ObservableCollection<MicroAttachment> fileItems = [];

        public ObservableCollection<MicroAttachment> FileItems
        {
            get => fileItems;
            set => SetProperty(ref fileItems, value);
        }

        private bool btnEnabled;

        public bool BtnEnabled {
            get => btnEnabled;
            set => SetProperty(ref btnEnabled, value);
        }


        public ICommand PickCommand { get; private set; }
        public ICommand SubmitCommand { get; private set; }

        private void TapPick()
        {
            PickImageAsync();
        }

        private void TapSubmit()
        {
            if (string.IsNullOrWhiteSpace(Content))
            {
                _notify.Warning("请输入内容");
                return;
            }
            BtnEnabled = false;
            _ = CreateAsync(new MicroForm()
            {
                Content = Content,
                OpenType = OpenType,
                File = FileItems
            });
        }

        private async Task CreateAsync(MicroForm form)
        {
            var data = await _api.CreateAsync(form);
            if (data == null)
            {
                return;
            }
            _app.DispatcherQueue.TryEnqueue(() => {
                _notify.Success("发布成功");
                _router.GoBack();
            });
        }

        private async void PickImageAsync()
        {
            var filePicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                FileTypeFilter =
                {
                    ".png", ".jpg", ".jpeg"
                }
            };
            _app.InitializePicker(filePicker);
            var files = await filePicker.PickMultipleFilesAsync();
            if (files.Count < 1)
            {
                return;
            }
            var data = await _upload.UploadImagesAsync(files);
            if (data == null)
            {
                return;
            }
            foreach (var item in data)
            {
                FileItems.Add(new MicroAttachment()
                {
                    Thumb = item.Thumb,
                    File = item.Url
                });
            }
        }

    }
}
