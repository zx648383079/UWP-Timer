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
using Windows.Foundation;
using Windows.Foundation.Collections;
using ZoDream.LogTimer.Extensions;
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
    public sealed partial class SharePage : Page
    {
        public SharePage()
        {
            this.InitializeComponent();
        }

        public MicroShareViewModel ViewModel = new MicroShareViewModel();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var data = e.Parameter as ShareData;
            ViewModel.Source = data;
        }

        private void RemoveFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as MicroAttachment;
            ViewModel.FileItems.Remove(item);
        }

        private void LargeHeader_Submited(object sender, TappedRoutedEventArgs e)
        {
            _ = CreateAsync(new MicroShareForm()
            {
                Content = ViewModel.Content,
                Pics = ViewModel.FileItems,
                Title = ViewModel.Title,
                Url = ViewModel.Url,
                Summary = ViewModel.Summary,
                Shareappid = ViewModel.Source.Appid,
                Sharesource = ViewModel.Source.Sharesource,
            });
        }

        private async Task CreateAsync(MicroShareForm form)
        {
            var data = await App.Repository.Micro.ShareSaveAsync(form);
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
