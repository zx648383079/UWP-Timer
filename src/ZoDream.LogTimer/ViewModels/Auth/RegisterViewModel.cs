using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;
using ZoDream.LogTimer.Services;
using static ZXing.QrCode.Internal.Mode;

namespace ZoDream.LogTimer.ViewModels
{
    internal class RegisterViewModel : ObservableObject
    {
        public RegisterViewModel()
        {
            SignUpCommand = new RelayCommand(TapSignUp);
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        private readonly INotificationService _notify = App.GetService<INotificationService>();
        private readonly NavigationService _router = App.GetService<NavigationService>();
        private readonly RestUserRepository _api = App.GetService<RestUserRepository>();
        private readonly IAuthService _auth = App.GetService<IAuthService>();

        private string nickname = string.Empty;

        public string Nickname {
            get => nickname;
            set => SetProperty(ref nickname, value);
        }


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

        private string confirmPassword = string.Empty;

        public string ConfirmPassword {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        private bool isAgree;

        public bool IsAgree {
            get => isAgree;
            set => SetProperty(ref isAgree, value);
        }

        private bool btnEnabled = true;

        public bool BtnEnabled {
            get => btnEnabled;
            set => SetProperty(ref btnEnabled, value);
        }

        public ICommand SignUpCommand { get; private set; }

        private void TapSignUp()
        {
            var form = new RegisterForm()
            {
                Name = Nickname.Trim(),
                Email = Account,
                Password = Password,
                RePassword = ConfirmPassword,
                Agree = IsAgree
            };
            if (!form.Agree)
            {
                _notify.Warning(App.GetString("login_agreement_error"));
                return;
            }
            if (!form.VerifyEmail())
            {
                _notify.Warning(App.GetString("login_email_error"));
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
            _ = RegisterAsync(form);
        }

        private async Task RegisterAsync(RegisterForm form)
        {
            BtnEnabled = false;
            _notify.Loading(true);
            var data = await _api.RegisterAsync(form, res => {
                _app.DispatcherQueue.TryEnqueue(() => {
                    _notify.Loading(false);
                    _notify.Error(res.Message);
                });

            });
            _app.DispatcherQueue.TryEnqueue(() => {
                BtnEnabled = true;
                _notify.Loading(false);
                if (data == null)
                {
                    return;
                }
                _auth.LoginAsync(data.Token, data);
                _notify.Error(App.GetString("login_reg_success_tip"));
                _router.Navigate(typeof(Pages.Plan.TodayPage));
            });

        }

    }
}
