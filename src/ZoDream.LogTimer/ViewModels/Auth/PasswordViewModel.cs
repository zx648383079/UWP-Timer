using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.LogTimer.Pages.Auth;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    internal class PasswordViewModel : ObservableObject
    {

        public PasswordViewModel()
        {
            ConfirmCommand = new RelayCommand(TapConfirm);
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        private readonly INotificationService _notify = App.GetService<INotificationService>();
        private readonly NavigationService _router = App.GetService<NavigationService>();
        private readonly RestUserRepository _api = App.GetService<RestUserRepository>();
        private readonly IAuthService _auth = App.GetService<IAuthService>();

        private string oldPassword = string.Empty;

        public string OldPassword {
            get => oldPassword;
            set => SetProperty(ref oldPassword, value);
        }


        private string password = string.Empty;

        public string Password {
            get => password;
            set => SetProperty(ref password, value);
        }

        private string confirmPassword = string.Empty;

        public string ConfirmPassword {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        public ICommand ConfirmCommand {  get; set; }

        private void TapConfirm()
        {
            var form = new PasswordForm()
            {
                OldPassword = OldPassword,
                Password = Password,
                RePassword = ConfirmPassword
            };
            if (!form.VerifyOldPassword())
            {
                _notify.Warning(App.GetString("pwd_old_error"));
                return;
            }
            if (!form.VerifyPassword())
            {
                _notify.Warning(App.GetString("pwd_new_error"));
                return;
            }
            if (form.Password != form.RePassword)
            {
                _notify.Warning(App.GetString("login_re_pwd_error"));
                return;
            }
            _ = EditPasswordAsync(form);
        }

        private async Task EditPasswordAsync(PasswordForm form)
        {
            _notify.Loading(true);
            var data = await _api.PasswordUpdateAsync(form, res => {
                _app.DispatcherQueue.TryEnqueue(() => {
                    _notify.Loading(false);
                    _notify.Error(res.Message);
                });

            });
            _notify.Loading(false);
            if (data == null)
            {
                return;
            }
            _auth.LogoutAsync();
            _notify.Success(App.GetString("pwd_update_success_tip"));
            _router.Navigate(typeof(LoginPage));
        }
    }
}
