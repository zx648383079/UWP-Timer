using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System.Display;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    internal class TaskDetailViewModel : ObservableObject, IDisposable
    {
        public TaskDetailViewModel()
        {
            StartCommand = new RelayCommand(TapStart);
            PauseCommand = new RelayCommand(TapPause);
            StopCommand = new RelayCommand(TapStop);
            MiniCommand = new RelayCommand(TapMini);
            _task.PausedChanged += Task_PausedChanged;
            _task.TimeUpdated += Task_TimeUpdated;
            ScreenOn = _setting.Get<bool>(nameof(ScreenOn));
            FullScreen = _setting.Get<bool>(nameof(FullScreen));
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        private readonly INotificationService _notify = App.GetService<INotificationService>();
        private readonly NavigationService _router = App.GetService<NavigationService>();
        private readonly RestTaskRepository _api = App.GetService<RestTaskRepository>();
        private readonly IAuthService _auth = App.GetService<IAuthService>();
        private readonly TaskService _task = App.GetService<TaskService>();
        private readonly ISettingService _setting = App.GetService<ISettingService>();
        private DisplayRequest? dispRequest = null;
        private readonly bool ScreenOn = false;
        private readonly bool FullScreen = false;

        private TaskDay today;

        public TaskDay Today {
            get => today;
            set => SetProperty(ref today, value);
        }

        private bool isRunning = false;

        public bool IsRunning {
            get => isRunning;
            set => SetProperty(ref isRunning, value);
        }

        private int duration;

        public int Duration {
            get => duration;
            set => SetProperty(ref duration, value);
        }

        private int progress;

        public int Progress {
            get => progress;
            set => SetProperty(ref progress, value);
        }

        private string taskName = string.Empty;

        public string TaskName {
            get => taskName;
            set => SetProperty(ref taskName, value);
        }

        private string taskDescription = string.Empty;

        public string TaskDescription {
            get => taskDescription;
            set => SetProperty(ref taskDescription, value);
        }



        public bool MiniEnabled => _task.Paused || Today.Id == _task.Id;

        public ICommand StartCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand MiniCommand { get; private set; }

        private void TapStart()
        {
            _ = _task.PlayAsync(Today.Id);
        }

        private void TapPause()
        {
            _ = _task.PauseAsync();
        }

        private void TapStop() 
        {
            _ = _task.StopAsync();
        }

        private void TapMini()
        {
            _task.SetTask(Today);
            _router.ShowTimer();
        }

        private void Task_TimeUpdated()
        {
            if (Today.Id == _task.Id)
            {

            }
            _app.DispatcherQueue.TryEnqueue(() => {
                Duration = _task.Duration;
                Progress = _task.Current;
            });
        }

        private void Task_PausedChanged()
        {
            if (Today.Id == _task.Id)
            {
                IsRunning = !_task.Paused;
                ApplyFullScreen(IsRunning);
                ApplyScreenOn(IsRunning);
            }
        }

        private async Task LoadTaskAsync(int id)
        {
            if (_task.Id == id && _task.Today is not null)
            {
                LoadTask(_task.Today, false);
                return;
            }
            _notify.Loading(true);
            var data = await _api.GetTaskDayDetailAsync(id);
            _app.DispatcherQueue.TryEnqueue(() => {
                _notify.Loading(false);
                if (data == null || data.Task == null)
                {
                    _router.GoBack();
                    return;
                }
                LoadTask(data);
            });
        }

        private void LoadTask(TaskDay data, bool sync = true)
        {
            Today = data;
            TaskName = data.Task.Name;
            TaskDescription = data.Task.Description;
            if (data.Log == null)
            {
                return;
            }
            Duration = data.Task.EveryTime;
            Progress = data.Log.Time;
            if (data.Log != null && data.Status == 9)
            {
                _task.Play(data);
            }
        }

        //private async Task ToggleVibrationAsync()
        //{
        //    //VibrationDevice vibrationDevice = await VibrationDevice.GetDefaultAsync();
        //    //vibrationDevice.SimpleHapticsController.SendHapticFeedbackForDuration()
        //}

        private void ApplyFullScreen(bool isFull)
        {
            if (!FullScreen)
            {
                return;
            }
            _app.FullScreenAsync(isFull);
        }

        private void ApplyScreenOn(bool isOn)
        {
            if (!ScreenOn)
            {
                return;
            }
            if (!isOn && dispRequest == null)
            {
                return;
            }
            if (!isOn)
            {
                dispRequest?.RequestRelease();
                dispRequest = null;
                return;
            }
            if (dispRequest == null)
            {
                dispRequest = new DisplayRequest();
                dispRequest.RequestActive();
            }
        }

        public void Load(int id)
        {
            Today = new TaskDay() { Id = id };
            _ = LoadTaskAsync(id);
        }

        public void Dispose()
        {
            ApplyFullScreen(false);
            ApplyScreenOn(false);
            _task.PausedChanged -= Task_PausedChanged;
            _task.TimeUpdated -= Task_TimeUpdated;
        }
    }
}
