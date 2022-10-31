using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Display;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using ZoDream.LogTimer.Controls;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Utils;
using ZoDream.LogTimer.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Plan
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        public DetailPage()
        {
            this.InitializeComponent();
        }

        private DisplayRequest dispRequest = null;

        public TodayViewModel ViewModel = new();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _ = LoadTask((int)e.Parameter);
            ViewModel.Settings = App.Store.UserOption;
            App.Store.Task.PausedChanged += Task_PausedChanged;
            App.Store.Task.TimeUpdated += Task_TimeUpdated;
        }

        private void Task_TimeUpdated(Stores.TaskStore sender)
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                progressBar.Max = sender.Duration;
                progressBar.Value = sender.Current;
            });
        }

        private void Task_PausedChanged(Stores.TaskStore sender)
        {
            if (ViewModel.Today.Id == sender.Id) 
            {
                ViewModel.IsRunning = !sender.Paused;
                FullScreen(ViewModel.IsRunning);
                ScreenOn(ViewModel.IsRunning);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            FullScreen(false);
            ScreenOn(false);
            App.Store.Task.PausedChanged -= Task_PausedChanged;
            App.Store.Task.TimeUpdated -= Task_TimeUpdated;
        }

        private async Task LoadTask(int id)
        {
            if (App.Store.Task.Id == id)
            {
                LoadTask(App.Store.Task.Today, false);
                return;
            }
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.Task.GetTaskDayDetailAsync(id);
            DispatcherQueue.TryEnqueue(() =>
            {
                App.ViewModel.IsLoading = false;
                if (data == null || data.Task == null)
                {
                    Frame.GoBack();
                    return;
                }
                LoadTask(data);
            });
        }

        private void LoadTask(TaskDay data, bool sync = true)
        {
            ViewModel.Today = data;
            nameTb.Text = data.Task.Name;
            descTb.Text = data.Task.Description;
            if (data.Log == null)
            {
                return;
            }
            progressBar.Max = data.Task.EveryTime;
            progressBar.Value = data.Log.Time;
            if (data.Log != null && data.Status == 9)
            {
                App.Store.Task.Play(data);
            }
        }

        //private async Task ToggleVibrationAsync()
        //{
        //    //VibrationDevice vibrationDevice = await VibrationDevice.GetDefaultAsync();
        //    //vibrationDevice.SimpleHapticsController.SendHapticFeedbackForDuration()
        //}

        private void FullScreen(bool isFull)
        {
            if (!ViewModel.Settings.FullScreen)
            {
                return;
            }
            return;
            var view = ApplicationView.GetForCurrentView();

            bool isInFullScreenMode = view.IsFullScreenMode;

            if (isFull == isInFullScreenMode)
            {
                return;
            }

            if (!isFull)
            {
                view.ExitFullScreenMode();
            }
            else
            {
                view.TryEnterFullScreenMode();
            }
        }

        private void ScreenOn(bool isOn)
        {
            if (!ViewModel.Settings.ScreenOn)
            {
                return;
            }
            if (!isOn && dispRequest == null)
            {
                return;
            }
            if (!isOn)
            {
                dispRequest.RequestRelease();
                dispRequest = null;
                return;
            }
            if (dispRequest == null)
            {
                dispRequest = new DisplayRequest();
                dispRequest.RequestActive();
            }
        }

        private void begin()
        {
            FullScreen(true);
            ScreenOn(true);
            ViewModel.IsRunning = true;
            progressBar.Max = ViewModel.Today.Task.EveryTime;
            progressBar.Value = ViewModel.Today.Log == null ? 0 : ViewModel.Today.Log.Time;
        }

        private void stop()
        {
            FullScreen(false);
            ScreenOn(false);
            ViewModel.IsRunning = false;
            progressBar.Max = 0;
            progressBar.Value = 0;
            progressBar.Stop();
        }

        private void playBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var label = (sender as IconButton).Name;
            if (label == "playBtn")
            {
                _ = App.Store.Task.PlayAsync(ViewModel.Today.Id);
                return;
            }
            if (label == "pauseBtn")
            {
                _ = App.Store.Task.PauseAsync();
                return;
            }
            if (label == "stopBtn")
            {
                _ = App.Store.Task.StopAsync();
                return;
            }
        }

        private void progressBar_TimeEnd(object sender, RangeBaseValueChangedEventArgs e)
        {
            _ = checkAsync();
        }

        private async Task checkAsync()
        {
            var data = await App.Repository.Task.CheckTaskAsync(ViewModel.Today.Id, res =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    _ = App.ViewModel.ShowMessageAsync(res.Message);
                });
            });
            if (data == null || data.Id < 1)
            {
                //Thread.Sleep(1000);
                //_ = checkAsync();
                return;
            }
            DispatcherQueue.TryEnqueue(() =>
            {
                ViewModel.Today = data;
                stop();
                Toast.ShowInfo(data.Tip);
                _ = App.ViewModel.ShowMessageAsync(data.Tip);
                if (data.Amount < 1)
                {
                    Frame.GoBack();
                }
            });
        }

        private async Task startAsync()
        {
            var data = await App.Repository.Task.PlayTaskAsync(ViewModel.Today.Id, res =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    _ = App.ViewModel.ShowMessageAsync(res.Message);
                });
            });
            if (data == null)
            {
                return;
            }
            DispatcherQueue.TryEnqueue(() =>
            {
                ViewModel.Today = data;
                begin();
            });
        }

        private async Task pauseAsync()
        {
            var data = await App.Repository.Task.PauseTaskAsync(ViewModel.Today.Id, res =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    _ = App.ViewModel.ShowMessageAsync(res.Message);
                });
            });
            if (data == null)
            {
                return;
            }
            DispatcherQueue.TryEnqueue(() =>
            {
                ViewModel.Today = data;
                ViewModel.IsRunning = false;
                progressBar.Stop();
            });
        }

        private async Task stopAsync()
        {
            var data = await App.Repository.Task.StopTaskAsync(ViewModel.Today.Id, res =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    _ = App.ViewModel.ShowMessageAsync(res.Message);
                });
            });
            if (data == null)
            {
                return;
            }
            DispatcherQueue.TryEnqueue(() =>
            {
                ViewModel.Today = data;
                stop();
            });
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            progressBar.Dispose();
            progressBar = null;
        }

        private void openComment_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = true;
        }

        private void ClosePanelBtn_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = false;
        }
    }
}
