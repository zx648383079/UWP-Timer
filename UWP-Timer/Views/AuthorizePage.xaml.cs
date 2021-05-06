using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWP_Timer.Repositories;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP_Timer.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AuthorizePage : Page
    {
        public AuthorizePage()
        {
            this.InitializeComponent();
        }

        private string token;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!App.IsLogin)
            {
                Frame.Navigate(typeof(Auth.LoginPage));
                return;
            }
            avatarImg.Source = Converters.ConverterHelper.ToImg(App.ViewModel.User.Avatar);
            token = e.Parameter as string;
            _ = checkTokenAsync(token);
        }

        private async Task checkTokenAsync(string v)
        {
            var model = await App.Repository.Authorize.AuthorizeQrTokenAsync(v);
            if (model == null)
            {
                var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
                await dispatcherQueue.EnqueueAsync(() =>
                {
                    _ = new MessageDialog(Constants.GetString("qr_is_expired")).ShowAsync();
                    Frame.GoBack();
                });
            }
        }

        private void confirmBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _ = confirm();
        }

        private void cancelBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _ = reject();
        }

        private async Task confirm()
        {
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            var model = await App.Repository.Authorize.AuthorizeQrTokenAsync(token, true);
            if (model == null)
            {
                await dispatcherQueue.EnqueueAsync(() =>
                {
                    _ = new MessageDialog(Constants.GetString("qr_is_expired")).ShowAsync();
                    Frame.GoBack();
                });
                return;
            }
            await dispatcherQueue.EnqueueAsync(() =>
            {
                Frame.Navigate(typeof(HomePage));
            });
        }

        private async Task reject()
        {
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            var model = await App.Repository.Authorize.AuthorizeQrTokenAsync(token, false, true);
            if (model == null)
            {
                await dispatcherQueue.EnqueueAsync(() =>
                {
                    _ = new MessageDialog(Constants.GetString("qr_is_expired")).ShowAsync();
                    Frame.GoBack();
                });
                return;
            }
            await dispatcherQueue.EnqueueAsync(() =>
            {
                Frame.GoBack();
            });
        }
    }
}
