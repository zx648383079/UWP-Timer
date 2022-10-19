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
using Windows.Foundation;
using Windows.Foundation.Collections;
using ZoDream.LogTimer.Controls;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Member
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IndexPage : Page
    {
        public IndexPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!App.IsLogin)
            {
                nameTb.Text = Constants.GetString("member_no_login_tip");
                avatarImg.Source = Converters.ConverterHelper.ToImg(string.Empty);
            }
            else if (App.ViewModel.User != null)
            {
                nameTb.Text = Constants.GetString("member_hi").Replace("{name}", App.ViewModel.User.Name);
                avatarImg.Source = Converters.ConverterHelper.ToImg(App.ViewModel.User.Avatar);
            }
            bulletinBtn.Visibility =
                checkBtn.Visibility =
                scanBtn.Visibility =
                App.IsLogin ? Visibility.Visible : Visibility.Collapsed;
        }

        private void IconMenuItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var label = (sender as IconLine).Name;
            var pageType = label == "scanBtn" ? typeof(ScanPage)
                : label == "checkBtn" ? typeof(CheckInPage)
                : label == "bulletinBtn" ? typeof(Message.IndexPage)
                : label == "settingBtn" ? typeof(SettingPage)
                : label == "helpBtn" ? typeof(Article.ListPage)
                : label == "fkBtn" ? typeof(FeedbackPage) : null;
            if (pageType != null)
            {
                Frame.Navigate(pageType);
            }
        }

        private void Border_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TapProfile();
        }

        private void nameTb_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TapProfile();
        }

        private void TapProfile()
        {
            if (App.IsLogin)
            {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("avatar", avatarImg);
            }
            Frame.Navigate(App.IsLogin ? typeof(ProfilePage) : typeof(Auth.LoginPage));
        }
    }
}
