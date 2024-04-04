using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vanara.PInvoke;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Services;
using ZXing.Aztec.Internal;

namespace ZoDream.LogTimer.ViewModels
{
    internal class AuthorizeViewModel: ObservableObject
    {
        public AuthorizeViewModel()
        {
            ConfirmCommand = new RelayCommand(TapConfirm);
            RejectCommand = new RelayCommand(TapReject);
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        private readonly INotificationService _notify = App.GetService<INotificationService>();
        private readonly NavigationService _router = App.GetService<NavigationService>();
        private readonly RestAuthorizeRepository _api = App.GetService<RestAuthorizeRepository>();
        private readonly IAuthService _auth = App.GetService<IAuthService>();

        private string _token = string.Empty;

        private string avatar = "/Assets/Square44x44Logo.scale-200.png";

        public string Avatar {
            get => avatar;
            set => SetProperty(ref avatar, value);
        }

        public ICommand ConfirmCommand { get; private set; }
        public ICommand RejectCommand { get; private set; }

        private void TapConfirm()
        {
            _ = ConfirmAsync();
        }

        private void TapReject()
        {
            _ = RejectAsync();
        }

        public void LoadAsync(string? token)
        {
            if (string.IsNullOrWhiteSpace(token) || !_auth.Authenticated)
            {
                _router.Navigate(typeof(Pages.Auth.LoginPage));
                return;
            }
            Avatar = _auth.AuthenticatedUser!.Avatar;
            _token = token;
            _ = CheckTokenAsync(token);
        }

        private async Task CheckTokenAsync(string v)
        {
            var model = await _api.AuthorizeQrTokenAsync(v);
            if (model == null)
            {
                _app.DispatcherQueue.TryEnqueue(() => {
                    _notify.Error(App.GetString("qr_is_expired"));
                    _router.GoBack();
                });
            }
        }

        private async Task ConfirmAsync()
        {
            var model = await _api.AuthorizeQrTokenAsync(_token, true);
            if (model == null)
            {
                _app.DispatcherQueue.TryEnqueue(() => {
                    _notify.Error(App.GetString("qr_is_expired"));
                    _router.GoBack();
                });
                return;
            }
            _app.DispatcherQueue.TryEnqueue(() => {
                _router.Navigate(typeof(Pages.Plan.TodayPage));
            });
        }

        private async Task RejectAsync()
        {
            var model = await _api.AuthorizeQrTokenAsync(_token, false, true);
            if (model == null)
            {
                _app.DispatcherQueue.TryEnqueue(() => {
                    _notify.Error(App.GetString("qr_is_expired"));
                    _router.GoBack();
                });
                return;
            }
            _app.DispatcherQueue.TryEnqueue(() => {
                _router.GoBack();
            });
        }
    }
}
