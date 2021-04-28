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
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP_Timer.Views.Member
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
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
