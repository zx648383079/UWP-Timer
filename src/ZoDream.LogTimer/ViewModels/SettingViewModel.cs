using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    internal class SettingViewModel : ObservableObject
    {

        public SettingViewModel()
        {
            ClearCommand = new RelayCommand(TapClear);
            Vibrate = _setting.Get<bool>(nameof(Vibrate));
            FullScreen = _setting.Get<bool>(nameof(FullScreen));
            ScreenOn = _setting.Get<bool>(nameof(ScreenOn));
            OpenUrlType = _setting.Get<int>(nameof(OpenUrlType));
            _enabled = true;
        }

        private readonly INotificationService _notify = App.GetService<INotificationService>();
        private readonly ISettingService _setting = App.GetService<ISettingService>();
        private readonly ICacheService _cache = App.GetService<ICacheService>();

        private bool _enabled = false;

        private bool vibrate;

        public bool Vibrate {
            get => vibrate;
            set {
                SetProperty(ref vibrate, value);
                if (!_enabled)
                {
                    return;
                }
                _setting.Set(nameof(Vibrate), value);
            }
        }

        private bool fullScreen;

        public bool FullScreen {
            get => fullScreen;
            set {
                SetProperty(ref fullScreen, value);
                if (!_enabled)
                {
                    return;
                }
                _setting.Set(nameof(FullScreen), value);
            }
        }

        private bool screenOn;

        public bool ScreenOn {
            get => screenOn;
            set {
                SetProperty(ref screenOn, value);
                if (!_enabled)
                {
                    return;
                }
                _setting.Set(nameof(ScreenOn), value);
            }
        }

        private int openUrlType = 1;

        public int OpenUrlType {
            get => openUrlType;
            set {
                SetProperty(ref openUrlType, value);
                if (!_enabled)
                {
                    return;
                }
                _setting.Set(nameof(OpenUrlType), value);
            }
        }



        private bool clearEnabled;

        public bool ClearEnabled {
            get => clearEnabled;
            set => SetProperty(ref clearEnabled, value);
        }

        public ICommand ClearCommand { get; private set; }

        private void TapClear()
        {
            _ = _cache.ClearAsync();
            _notify.Success(App.GetString("clear_successfully"));
            ClearEnabled = false;
        }
    }
}
