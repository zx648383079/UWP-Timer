using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;
using ZoDream.LogTimer.Services;
using static System.Net.Mime.MediaTypeNames;
using static ZXing.QrCode.Internal.Mode;

namespace ZoDream.LogTimer.ViewModels
{
    internal class AccountEditViewModel : ObservableObject
    {
        public AccountEditViewModel()
        {
            SubmitCommand = new RelayCommand(TapSubmit);
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

        private int sex;

        public int Sex {
            get => sex;
            set => SetProperty(ref sex, value);
        }

        private DateTime birthday;

        public DateTime Birthday {
            get => birthday;
            set => SetProperty(ref birthday, value);
        }


        public ICommand SubmitCommand { get; private set; }

        private void TapSubmit()
        {
            var form = new ProfileForm()
            {
                Name = Nickname,
                Sex = Sex,
                Birthday = Birthday.ToString("yyyy-MM-dd")
            };
            if (string.IsNullOrWhiteSpace(form.Name))
            {
                _notify.Warning(App.GetString("edit_name_error"));
                return;
            }
            _ = EditProfileAsync(form);
        }

        private async Task EditProfileAsync(ProfileForm form)
        {
            _notify.Loading(true);
            var data = await _api.UpdateAsync(form, res => {
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
                _auth.LoginAsync(data);
                _router.GoBack();
            });

        }

        public void Load()
        {
            if (!_auth.Authenticated || _auth.AuthenticatedUser is null)
            {
                _router.GoBack();
                return;
            }
            var user = _auth.AuthenticatedUser;
            Nickname = user.Name;
            Sex = user.Sex;
            Birthday = DateTime.Parse(user.Birthday);
        }
    }
}
