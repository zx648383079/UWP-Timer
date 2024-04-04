using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    internal class FindViewModel : ObservableObject
    {
        public FindViewModel()
        {
            SendCommand = new RelayCommand(TapSend);
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        private readonly INotificationService _notify = App.GetService<INotificationService>();
        private readonly NavigationService _router = App.GetService<NavigationService>();
        private readonly RestUserRepository _api = App.GetService<RestUserRepository>();
        private readonly IAuthService _auth = App.GetService<IAuthService>();

        private string account = string.Empty;

        public string Account {
            get => account;
            set => SetProperty(ref account, value);
        }

        private string password = string.Empty;

        public string Password {
            get => password;
            set => SetProperty(ref password, value);
        }

        private string code = string.Empty;

        public string Code {
            get => code;
            set => SetProperty(ref code, value);
        }

        private string confirmPassword = string.Empty;

        public string ConfirmPassword {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        private bool sendMode = true;

        public bool SendMode {
            get => sendMode;
            set {
                SetProperty(ref sendMode, value);
                OnPropertyChanged(nameof(BtnText));
            }
        }

        public string BtnText => App.GetString(SendMode ? "login_send_email" : "login_reset_label");

        public ICommand SendCommand { get; private set; }

        private void TapSend()
        {
            var form = new RegisterForm()
            {
                Email = Account,
                Code = Code,
                Password = Password,
                RePassword = ConfirmPassword
            };
            if (!form.VerifyEmail())
            {
                _notify.Warning(App.GetString("login_email_error"));
                return;
            }
            if (SendMode)
            {
                _ = SendEmailAsync(form.Email);
                return;
            }
            if (form.Code.Length < 4)
            {
                _notify.Warning(App.GetString("login_code_error"));
                return;
            }
            if (!form.VerifyPassword())
            {
                _notify.Warning(App.GetString("login_pwd_error"));
                return;
            }
            if (form.Password != form.RePassword)
            {
                _notify.Warning(App.GetString("login_re_pwd_error"));
                return;
            }
            _ = ResetPasswordAsync(form);
        }

        private async Task ResetPasswordAsync(RegisterForm form)
        {
            _notify.Loading(true);
            var data = await _api.ResetAsync(form, res => {
                _app.DispatcherQueue.TryEnqueue(() => {
                    _notify.Loading(false);
                    _notify.Error(res.Message);
                });

            });
            _app.DispatcherQueue.TryEnqueue(() => {
                _notify.Loading(false);
                if (data == null)
                {
                    return;
                }
                _notify.Success(App.GetString("login_reset_success_tip"));
                _router.GoBack();
            });

        }

        private async Task SendEmailAsync(string email)
        {
            _notify.Loading(true);
            var data = await _api.SendFindEmailAsync(email, res => {
                _app.DispatcherQueue.TryEnqueue(() => {
                    _notify.Loading(false);
                    _notify.Error(res.Message);
                });

            });

            _app.DispatcherQueue.TryEnqueue(() => {
                _notify.Loading(false);
                if (data == null || !data.Data)
                {
                    return;
                }
                _notify.Success(App.GetString("login_send_success_tip"));
                SendMode = false;
            });
        }
    }
}
