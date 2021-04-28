using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Timer.Controls;
using UWP_Timer.Repositories;
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
    public sealed partial class IndexPage : Page
    {
        public IndexPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!App.IsLogin())
            {
                nameTb.Text = Constants.GetString("member_no_login_tip");
                avatarImg.Source = Converters.ConverterHelper.ToImg(string.Empty);
            } else if(App.ViewModel.User != null)
            {
                nameTb.Text = Constants.GetString("member_hi").Replace("{name}", App.ViewModel.User.Name);
                avatarImg.Source = Converters.ConverterHelper.ToImg(App.ViewModel.User.Avatar);
            }
            bulletinBtn.Visibility = 
                checkBtn.Visibility = 
                scanBtn.Visibility = 
                App.IsLogin() ? Visibility.Visible : Visibility.Collapsed;
            
        }

        private void IconMenuItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var label = (sender as IconMenuItem).Name;
            var pageType = label == "scanBtn" ? typeof(ScanPage)
                : label == "checkBtn" ? typeof(CheckInPage)
                : label == "bulletinBtn" ? typeof(Message.IndexPage)
                : label == "settingBtn" ? typeof(SettingPage)
                : label == "helpBtn" ? typeof(Article.IndexPage)
                : label == "fkBtn" ? typeof(FeedbackPage) : null;
            if (pageType != null)
            {
                Frame.Navigate(pageType);
            }
        }

        private void Border_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(App.IsLogin() ? typeof(ProfilePage) : typeof(LoginPage));
        }

        private void nameTb_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(App.IsLogin() ? typeof(ProfilePage) : typeof(LoginPage));
        }
    }
}
