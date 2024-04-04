using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.LogTimer.Pages.Member;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Services;
using static System.Net.Mime.MediaTypeNames;
using static ZXing.QrCode.Internal.Mode;

namespace ZoDream.LogTimer.ViewModels
{
    internal class ProfileViewModel : ObservableObject
    {
        public ProfileViewModel()
        {
            SignOutCommand = new RelayCommand(TapSignOut);
            EditCommand = new RelayCommand(TapEdit);
            AvatarCommand = new RelayCommand(TapAvatar);
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

        private string avatar = "/Assets/Square44x44Logo.scale-200.png";

        public string Avatar {
            get => avatar;
            set => SetProperty(ref avatar, value);
        }



        private string sex;

        public string Sex {
            get => sex;
            set => SetProperty(ref sex, value);
        }

        private string birthday;

        public string Birthday {
            get => birthday;
            set => SetProperty(ref birthday, value);
        }

        public ICommand AvatarCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand SignOutCommand { get; private set; }

        private void TapEdit()
        {
            if (_auth.AuthenticatedUser is null)
            {
                return;
            }
            _router.Navigate(typeof(EditPage));
        }

        private void TapAvatar()
        {
            if (_auth.AuthenticatedUser is null)
            {
                return;
            }
            _router.Navigate(typeof(AvatarPage));
        }

        private void TapSignOut()
        {
            _ = LogoutAsync();
        }

        private async Task LogoutAsync()
        {
            _notify.Loading(true);
            var data = await _api.LogoutAsync();
            _app.DispatcherQueue.TryEnqueue(() => {
                _notify.Loading(false);
                _auth.LogoutAsync();
                _router.Navigate(typeof(Pages.Plan.TodayPage));
            });
        }

        public bool Load()
        {
            if (!_auth.Authenticated || _auth.AuthenticatedUser == null)
            {
                _router.GoBack();
                return false;
            }
            var user = _auth.AuthenticatedUser;
            Avatar = user.Avatar;
            Nickname = user.Name;
            Sex = user.SexLabel;
            Birthday = user.Birthday;
            return true;
        }
    }
}
