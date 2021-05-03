using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWP_Timer.Models;
using UWP_Timer.Utils;
using UWP_Timer.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP_Timer.Views.Micro
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
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
            _ = PickImage();
        }

        private async Task PickImage()
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
                Conent = ViewModel.Content,
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
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            await dispatcherQueue.EnqueueAsync(() =>
            {
                Toast.Tip("发布成功");
                Frame.GoBack();
            });
        }
    }
}
