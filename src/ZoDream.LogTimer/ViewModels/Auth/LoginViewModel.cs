using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    internal class LoginViewModel : ObservableObject
    {
        public LoginViewModel()
        {
            SignInCommand = new RelayCommand(TapSignIn);
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

        private bool btnEnabled = true;

        public bool BtnEnabled {
            get => btnEnabled;
            set => SetProperty(ref btnEnabled, value);
        }



        public ICommand SignInCommand { get; private set; }

        private void TapSignIn()
        {
            var form = new LoginForm()
            {
                Email = Account,
                Password = Password,
                Remember = true
            };
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
            _ = LoginAsync(form);
        }

        private async Task LoginAsync(LoginForm form)
        {
            _notify.Loading(true);
            BtnEnabled = false;
            var data = await _api.LoginAsync(form, res => {
                _app.DispatcherQueue.TryEnqueue(() => {
                    _notify.Loading(false);
                    _notify.Error(res.Message);
                });
            });
            _app.DispatcherQueue.TryEnqueue(() => {
                _notify.Loading(false);
                BtnEnabled = true;
                if (data == null)
                {
                    return;
                }
                _auth.LoginAsync(data.Token, data);
                _router.GoBack();
            });
        }
    }
}
