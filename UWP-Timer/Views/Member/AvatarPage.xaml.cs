using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWP_Timer.Repositories;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP_Timer.Views.Member
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AvatarPage : Page
    {
        public AvatarPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _ = PickImage();
        }

        private void LargeHeader_Submited(object sender, TappedRoutedEventArgs e)
        {
            _ = SaveAsync();
        }


        private async Task SaveAsync()
        {
            var stream = new InMemoryRandomAccessStream();
            await ImageCropper.SaveAsync(stream, BitmapFileFormat.Png);
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.User.UploadAvatarAsync(stream, async res =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    App.ViewModel.IsLoading = false;
                    _ = new MessageDialog(res.Message ?? Constants.GetString("unknow_error")).ShowAsync();
                });

            });
            stream.Dispose();
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                App.ViewModel.IsLoading = false;
                if (data == null)
                {
                    return;
                }
                App.ViewModel.User = data;
                Frame.GoBack();
            });
            
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
            var file = await filePicker.PickSingleFileAsync();
            if (file != null && ImageCropper != null)
            {
                await ImageCropper.LoadImageFromFile(file);
                return;
            }
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Frame.GoBack();
            });
        }
    }
}
