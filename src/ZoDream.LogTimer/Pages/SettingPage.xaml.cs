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
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
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
            var data = App.Store.UserOption;
            vibrateTs.IsOn = data.Vibrate;
            fullScreenTs.IsOn = data.FullScreen;
            screenOnTs.IsOn = data.ScreenOn;
        }

        private void LargeHeader_Submited(object sender, TappedRoutedEventArgs e)
        {
            var data = new UserOption()
            {
                Vibrate = vibrateTs.IsOn,
                FullScreen = fullScreenTs.IsOn,
                ScreenOn = screenOnTs.IsOn
            };
            App.Store.SaveAsync(data);
            _ = App.ViewModel.ShowMessageAsync(Constants.GetString("setting_save_success"));
        }

        private void CacheBtn_Click(object sender, RoutedEventArgs e)
        {
            _ = Cache.ClearAsync();
            Toast.Tip("清除成功！");
            CacheBtn.IsEnabled = false;
        }
    }
}
