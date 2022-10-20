using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
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
using Windows.UI.Popups;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Auth
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
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
            var data = await App.Repository.User.LoginAsync(form, res =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    App.ViewModel.IsLoading = false;
                    _ = new MessageDialog(res.Message).ShowAsync();
                });

            });
            DispatcherQueue.TryEnqueue(() =>
            {
                App.ViewModel.IsLoading = false;
                LoginBtn.IsTapEnabled = true;
                if (data == null)
                {
                    return;
                }
                App.Store.Auth.LoginAsync(data.Token, data);
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
