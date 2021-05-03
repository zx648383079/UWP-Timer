using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWP_Timer.Models;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP_Timer.Views.Auth
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("avatar");
            if (imageAnimation != null)
            {
                imageAnimation.TryStart(LogoImg);
            }
        }

        private void toRegisterBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("logo", LogoImg);
            Frame.Navigate(typeof(RegisterPage));
        }

        private void toFindBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("logo", LogoImg);
            Frame.Navigate(typeof(FindPage));
        }

        private void LoginBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var form = new LoginForm()
            {
                Email = emailTb.Text,
                Password = pwdTb.Password,
                Remember = true
            };
            if (!form.VerifyEmail())
            {
                _ = new MessageDialog(Constants.GetString("login_email_error")).ShowAsync();
                return;
            }
            if (!form.VerifyPassword())
            {
                _ = new MessageDialog(Constants.GetString("login_pwd_error")).ShowAsync();
                return;
            }
            _ = Login(form);
        }

        private async Task Login(LoginForm form)
        {
            App.ViewModel.IsLoading = true;
            LoginBtn.IsTapEnabled = false;
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            var data = await App.Repository.User.LoginAsync(form, async res =>
            {
                await dispatcherQueue.EnqueueAsync(() =>
                {
                    App.ViewModel.IsLoading = false;
                    _ = new MessageDialog(res.Message).ShowAsync();
                });
                
            });
            await dispatcherQueue.EnqueueAsync(() =>
            {
                App.ViewModel.IsLoading = false;
                LoginBtn.IsTapEnabled = true;
                if (data == null)
                {
                    return;
                }
                App.Login(data);
                Frame.GoBack();
            });
        }

        private void pwdTb_GotFocus(object sender, RoutedEventArgs e)
        {
            LogoImg.Status = Controls.LogoStatus.INIT;
        }

        private void pwdTb_LostFocus(object sender, RoutedEventArgs e)
        {
            LogoImg.Status = Controls.LogoStatus.NONE;
        }

        private void QrBtn_Click(object sender, RoutedEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("logo", LogoImg);
            Frame.Navigate(typeof(QrPage));
        }
    }
}
