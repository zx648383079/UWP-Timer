using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using UWP_Timer.Controls;
using UWP_Timer.Utils;
using UWP_Timer.ViewModels;
using Windows.Devices.Haptics;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Display;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP_Timer.Views.Tasks
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        public DetailPage()
        {
            this.InitializeComponent();
        }

        private DisplayRequest dispRequest = null;

        public TodayViewModel ViewModel = new TodayViewModel();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _ = LoadTask((int)e.Parameter);
            ViewModel.Settings = App.ViewModel.GetSettings();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            FullScreen(false);
            ScreenOn(false);
            progressBar.Stop();
        }

        private async Task LoadTask(int id)
        {
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.Task.GetTaskDayDetailAsync(id);
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                App.ViewModel.IsLoading = false;
                if (data == null || data.Task == null)
                {
                    Frame.GoBack();
                    return;
                }
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
                    begin();
                }
            });
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
            progressBar.Start();
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
                _ = startAsync();
                return;
            }
            if (label == "pauseBtn")
            {
                _ = pauseAsync();
                return;
            }
            if (label == "stopBtn")
            {
                _ = stopAsync();
                return;
            }
        }

        private void progressBar_TimeEnd(object sender, RangeBaseValueChangedEventArgs e)
        {
            _ = checkAsync();
        }

        private async Task checkAsync()
        {
            var data = await App.Repository.Task.CheckTaskAsync(ViewModel.Today.Id, async res =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    _ = new MessageDialog(res.Message).ShowAsync();
                });
            });
            if (data == null || data.Id < 1)
            {
                //Thread.Sleep(1000);
                //_ = checkAsync();
                return;
            }
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                ViewModel.Today = data;
                stop();
                Toast.ShowInfo(data.Tip);
                _ = new MessageDialog(data.Tip).ShowAsync();
                if (data.Amount < 1)
                {
                    Frame.GoBack();
                }
            });
        }

        private async Task startAsync()
        {
            var data = await App.Repository.Task.PlayTaskAsync(ViewModel.Today.Id, async res =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    _ = new MessageDialog(res.Message).ShowAsync();
                });
            });
            if (data == null)
            {
                return;
            }
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                ViewModel.Today = data;
                begin();
            });
        }

        private async Task pauseAsync()
        {
            var data = await App.Repository.Task.PauseTaskAsync(ViewModel.Today.Id, async res =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    _ = new MessageDialog(res.Message).ShowAsync();
                });
            });
            if (data == null)
            {
                return;
            }
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                ViewModel.Today = data;
                ViewModel.IsRunning = false;
                progressBar.Stop();
            });
        }

        private async Task stopAsync()
        {
            var data = await App.Repository.Task.StopTaskAsync(ViewModel.Today.Id, async res =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    _ = new MessageDialog(res.Message).ShowAsync();
                });
            });
            if (data == null)
            {
                return;
            }
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                ViewModel.Today = data;
                stop();
            });
        }
    }
}
