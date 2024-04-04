using Microsoft.UI.Xaml;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinRT.Interop;
using Microsoft.UI.Windowing;
using Windows.ApplicationModel;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    internal partial class AppViewModel
    {

        private Window _baseWindow;
        private IntPtr _baseWindowHandle;
        private AppWindow _appWindow;
        public Window BaseWindow {
            set {
                _baseWindow = value;
                _baseWindowHandle = WindowNative.GetWindowHandle(_baseWindow);
                var windowId = Win32Interop.GetWindowIdFromWindow(_baseWindowHandle);
                _appWindow = AppWindow.GetFromWindowId(windowId);
            }
        }

        /// <summary>
        /// UI线程.
        /// </summary>
        public DispatcherQueue DispatcherQueue => _baseWindow!.DispatcherQueue;

        public XamlRoot BaseXamlRoot => _baseWindow!.Content.XamlRoot;

        /// <summary>
        /// 获取当前版本号.
        /// </summary>
        /// <returns>版本号.</returns>
        public string Version {
            get {
                var version = Package.Current.Id.Version;
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
        }

        public void InitializePicker(object target)
        {
            InitializeWithWindow.Initialize(target, _baseWindowHandle);
        }

        public IAsyncOperation<ContentDialogResult> OpenDialogAsync(ContentDialog target)
        {
            target.XamlRoot = BaseXamlRoot;
            return target.ShowAsync();
        }

        public void InitializeTheme()
        {
            //var localTheme = App.GetService<ISettingRepository>().Get(SettingNames.AppTheme, AppConstants.ThemeDefault);
            //if (localTheme != AppConstants.ThemeDefault)
            //{
            //    Application.Current.RequestedTheme = localTheme == AppConstants.ThemeLight ?
            //                            ApplicationTheme.Light :
            //                            ApplicationTheme.Dark;
            //}
        }


        public void FullScreenAsync(bool isFull)
        {
            _appWindow.SetPresenter(isFull ? AppWindowPresenterKind.FullScreen : AppWindowPresenterKind.Default);
        }

        public async Task ShowMessageAsync(string message, string title = "提示")
        {
            var dialog = new ContentDialog()
            {
                Title = title,
                Content = message,
                CloseButtonText = "Ok",
            };
            await OpenDialogAsync(dialog);
        }

    }
}
