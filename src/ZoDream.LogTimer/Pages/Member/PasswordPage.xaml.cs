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
using ZoDream.LogTimer.Pages.Auth;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;

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

        private void LargeHeader_Submitted(object sender, TappedRoutedEventArgs e)
        {
            var form = new PasswordForm()
            {
                OldPassword = oldPwdTb.Password,
                Password = pwdTb.Password,
                RePassword = rePwdTb.Password
            };
            if (!form.VerifyOldPassword())
            {
                _ = App.ViewModel.ShowMessageAsync(Constants.GetString("pwd_old_error"));
                return;
            }
            if (!form.VerifyPassword())
            {
                _ = App.ViewModel.ShowMessageAsync(Constants.GetString("pwd_new_error"));
                return;
            }
            if (form.Password != form.RePassword)
            {
                _ = App.ViewModel.ShowMessageAsync(Constants.GetString("login_re_pwd_error"));
                return;
            }
            _ = EditPasswordAsync(form);
        }

        private async Task EditPasswordAsync(PasswordForm form)
        {
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.User.PasswordUpdateAsync(form, res =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    App.ViewModel.IsLoading = false;
                    _ = App.ViewModel.ShowMessageAsync(res.Message);
                });

            });
            App.ViewModel.IsLoading = false;
            if (data == null)
            {
                return;
            }
            App.Store.Auth.LogoutAsync();
            _ = App.ViewModel.ShowMessageAsync(Constants.GetString("pwd_update_success_tip"));
            Frame.Navigate(typeof(LoginPage));
        }
    }
}
