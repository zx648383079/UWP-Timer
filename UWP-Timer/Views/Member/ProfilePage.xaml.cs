using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWP_Timer.Controls;
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

namespace UWP_Timer.Views.Member
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!App.IsLogin() || App.ViewModel.User == null)
            {
                Frame.GoBack();
                return;
            }
            var user = App.ViewModel.User;
            avatarImg.Source = Converters.ConverterHeler.ToImg(user.Avatar);
            nameTb.Tip = user.Name;
            sexTb.Tip = user.SexLabel;
            birthdayTb.Tip = user.Birthday;
        }

        private void nameTb_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.ViewModel.User ==  null)
            {
                return;
            }
            Frame.Navigate(typeof(EditPage));
        }

        private void avatar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.ViewModel.User == null)
            {
                return;
            }
            Frame.Navigate(typeof(AvatarPage));
        }

        private void TipMenuItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var label = (sender as TipMenuItem).Name;
            var pageType = label == "pwdBtn" ? typeof(PasswordPage)
                : label == "connectBtn" ? typeof(Account.ConnectPage)
                : label == "driverBtn" ? typeof(Account.DriverPage)
                : label == "cancelBtn" ? typeof(Account.CancelPage)
                : null;
            if (pageType != null)
            {
                Frame.Navigate(pageType);
            }
        }

        private void logoutBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _ = Logout();
        }

        private async Task Logout()
        {
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.User.LogoutAsync();
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                App.ViewModel.IsLoading = false;
                App.Logout();
                Frame.Navigate(typeof(HomePage));
            });
        }
    }
}
