using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static System.Net.Mime.MediaTypeNames;
using static ZXing.QrCode.Internal.Mode;
using ZoDream.LogTimer.Extensions;
using ZoDream.LogTimer.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Member
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
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
            if (!App.IsLogin || App.ViewModel.User == null)
            {
                Frame.GoBack();
                return;
            }
            var user = App.ViewModel.User;
            avatarImg.Source = Converters.ConverterHelper.ToImg(user.Avatar);
            nameTb.Content = user.Name;
            sexTb.Content = user.SexLabel;
            birthdayTb.Content = user.Birthday;
            ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("avatar");
            if (imageAnimation != null)
            {
                imageAnimation.TryStart(avatarImg);
            }
        }

        private void nameTb_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.ViewModel.User == null)
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
            var label = (sender as IconLine).Name;
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
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            await dispatcherQueue.EnqueueAsync(() =>
            {
                App.ViewModel.IsLoading = false;
                App.Logout();
                Frame.Navigate(typeof(Plan.TodayPage));
            });
        }
    }
}
