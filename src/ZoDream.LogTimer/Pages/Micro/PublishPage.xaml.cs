using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Vanara.PInvoke;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Utils;
using ZoDream.LogTimer.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Micro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PublishPage : Page
    {
        public PublishPage()
        {
            this.InitializeComponent();
        }

        public MicroPublishViewModel ViewModel = new MicroPublishViewModel();

        private void RemoveFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as MicroAttachment;
            ViewModel.FileItems.Remove(item);
        }

        private void AddFileBtn_Click(object sender, RoutedEventArgs e)
        {
            PickImage();
        }

        private async void PickImage()
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            var filePicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                FileTypeFilter =
                {
                    ".png", ".jpg", ".jpeg"
                }
            };
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);
            var files = await filePicker.PickMultipleFilesAsync();
            if (files.Count < 1)
            {
                return;
            }
            var data = await App.Repository.File.UploadImagesAsync(files);
            if (data == null)
            {
                return;
            }
            foreach (var item in data)
            {
                ViewModel.FileItems.Add(new MicroAttachment()
                {
                    Thumb = item.Thumb,
                    File = item.Url
                });
            }
        }

        private void EmojiBox_SelectionChanged(Controls.EmojiBox sender, Controls.EmojiTappedArgs args)
        {
            ContentTb.SelectedText = args.Emoji.Type > 0 ? args.Emoji.Content : $"[{args.Emoji.Name}]";
            emojiFlyout.Hide();
        }

        private void PublishBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ViewModel.Content))
            {
                Toast.Tip("请输入内容");
                return;
            }
            PublishBtn.IsEnabled = false;
            _ = CreateAsync(new MicroForm()
            {
                Content = ViewModel.Content,
                OpenType = OpenCb.SelectedIndex,
                File = ViewModel.FileItems
            });
        }

        private async Task CreateAsync(MicroForm form)
        {
            var data = await App.Repository.Micro.CreateAsync(form);
            if (data == null)
            {
                return;
            }
            DispatcherQueue.TryEnqueue(() =>
            {
                Toast.Tip("发布成功");
                Frame.GoBack();
            });
        }
    }
}
