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
    public sealed partial class FindPage : Page
    {
        public FindPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("logo");
            if (imageAnimation != null)
            {
                imageAnimation.TryStart(LogoImg);
            }
        }

        private void backBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void sendBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var isSend = findBox.Visibility == Visibility.Collapsed;
            var form = new RegisterForm()
            {
                Email = emailTb.Text,
                Code = codeTb.Text,
                Password = pwdTb.Password,
                RePassword = rePwdTb.Password
            };
            if (!form.VerifyEmail())
            {
                _ = App.ViewModel.ShowMessageAsync(Constants.GetString("login_email_error"));
                return;
            }
            if (isSend)
            {
                _ = SendEmail(form.Email);
                return;
            }
            if (form.Code.Length < 4)
            {
                _ = App.ViewModel.ShowMessageAsync(Constants.GetString("login_code_error"));
                return;
            }
            if (!form.VerifyPassword())
            {
                _ = App.ViewModel.ShowMessageAsync(Constants.GetString("login_pwd_error"));
                return;
            }
            if (form.Password != form.RePassword)
            {
                _ = App.ViewModel.ShowMessageAsync(Constants.GetString("login_re_pwd_error"));
                return;
            }
            _ = ResetPassword(form);
        }

        private async Task ResetPassword(RegisterForm form)
        {
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.User.ResetAsync(form, res =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    App.ViewModel.IsLoading = false;
                    _ = App.ViewModel.ShowMessageAsync(res.Message);
                });

            });
            DispatcherQueue.TryEnqueue(() =>
            {
                App.ViewModel.IsLoading = false;
                if (data == null)
                {
                    return;
                }
                _ = App.ViewModel.ShowMessageAsync(Constants.GetString("login_reset_success_tip"));
                Frame.GoBack();
            });

        }

        private async Task SendEmail(string email)
        {
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.User.SendFindEmailAsync(email, res =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    App.ViewModel.IsLoading = false;
                    _ = App.ViewModel.ShowMessageAsync(res.Message);
                });

            });

            DispatcherQueue.TryEnqueue(() =>
            {
                App.ViewModel.IsLoading = false;
                if (data == null || !data.Data)
                {
                    return;
                }
                _ = App.ViewModel.ShowMessageAsync(Constants.GetString("login_send_success_tip"));
                findBox.Visibility = Visibility.Visible;
                sendBtn.Content = Constants.GetString("login_reset_label");
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
    }
}
