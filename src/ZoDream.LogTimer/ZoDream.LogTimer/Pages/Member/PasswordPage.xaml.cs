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
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Pages.Auth;
using ZoDream.LogTimer.Repositories;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Member
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PasswordPage : Page
    {
        public PasswordPage()
        {
            this.InitializeComponent();
        }

        private void LargeHeader_Submited(object sender, TappedRoutedEventArgs e)
        {
            var form = new PasswordForm()
            {
                OldPassword = oldPwdTb.Password,
                Password = pwdTb.Password,
                RePassword = rePwdTb.Password
            };
            if (!form.VerifyOldPassword())
            {
                _ = new MessageDialog(Constants.GetString("pwd_old_error")).ShowAsync();
                return;
            }
            if (!form.VerifyPassword())
            {
                _ = new MessageDialog(Constants.GetString("pwd_new_error")).ShowAsync();
                return;
            }
            if (form.Password != form.RePassword)
            {
                _ = new MessageDialog(Constants.GetString("login_re_pwd_error")).ShowAsync();
                return;
            }
            _ = EditPasswordAsync(form);
        }

        private async Task EditPasswordAsync(PasswordForm form)
        {
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.User.PasswordUpdateAsync(form, async res =>
            {
                var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
                await dispatcherQueue.EnqueueAsync(() =>
                {
                    App.ViewModel.IsLoading = false;
                    _ = new MessageDialog(res.Message).ShowAsync();
                });

            });
            App.ViewModel.IsLoading = false;
            if (data == null)
            {
                return;
            }
            App.Logout();
            _ = new MessageDialog(Constants.GetString("pwd_update_success_tip")).ShowAsync();
            Frame.Navigate(typeof(LoginPage));
        }
    }
}
