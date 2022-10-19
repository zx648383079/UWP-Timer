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
using Windows.UI.Popups;
using ZoDream.LogTimer.Extensions;
using ZoDream.LogTimer.Repositories;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
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
                Frame.Navigate(typeof(Plan.TodayPage));
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
