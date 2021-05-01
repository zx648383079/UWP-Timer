using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Timer.Models;
using UWP_Timer.Repositories;
using UWP_Timer.Utils;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP_Timer.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var data = App.ViewModel.GetSettings();
            vibrateTs.IsOn = data.Vibrate;
            fullScreenTs.IsOn = data.FullScreen;
            screenOnTs.IsOn = data.ScreenOn;
        }

        private void LargeHeader_Submited(object sender, TappedRoutedEventArgs e)
        {
            var data = new SettingItem()
            {
                Vibrate = vibrateTs.IsOn,
                FullScreen = fullScreenTs.IsOn,
                ScreenOn = screenOnTs.IsOn
            };
            App.ViewModel.SetSettings(data);
            _ = new MessageDialog(Constants.GetString("setting_save_success")).ShowAsync();
        }

        private void CacheBtn_Click(object sender, RoutedEventArgs e)
        {
            _ = Cache.ClearAsync();
            Toast.Tip("清除成功！");
            CacheBtn.IsEnabled = false;
        }
    }
}
