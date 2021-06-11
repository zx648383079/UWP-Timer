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
