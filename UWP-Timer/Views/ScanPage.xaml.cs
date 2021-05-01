using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWP_Timer.Repositories;
using UWP_Timer.Utils;
using Windows.ApplicationModel;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ZXing;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP_Timer.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ScanPage : Page
    {
        BarcodeReader barcodeReader;
        bool IsBusy = false;
        DispatcherQueue dispatcherQueue;

        public ScanPage()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            Application.Current.Suspending += Application_Suspending;
            Application.Current.Resuming += Application_Resuming;
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
                return;
            }
        }

        private async void Application_Suspending(object sender, SuspendingEventArgs e)
        {
            // Handle global application events only if this page is active
            if (Frame.CurrentSourcePageType == typeof(MainPage))
            {
                var deferral = e.SuspendingOperation.GetDeferral();

                await CleanupCameraAsync();

                deferral.Complete();
            }

        }

        private void Application_Resuming(object sender, object o)
        {
            // Handle global application events only if this page is active
            if (Frame.CurrentSourcePageType == typeof(MainPage))
            {
            }
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            await CleanupCameraAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _ = InitCameraAsync();
        }

        private async Task InitCameraAsync()
        {
            UnsubscribeFromEvents();
            dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            if (CameraPreviewControl != null)
            {
                CameraPreviewControl.PreviewFailed += CameraPreviewControl_PreviewFailed;
                await CameraPreviewControl.StartAsync();
                CameraPreviewControl.CameraHelper.FrameArrived += CameraPreviewControl_FrameArrived;
                barcodeReader = new BarcodeReader();
            }

        }

        private void CameraPreviewControl_FrameArrived(object sender, Microsoft.Toolkit.Uwp.Helpers.FrameEventArgs e)
        {
            _ = SanFrameAsync(e.VideoFrame);
        }

        private async Task SanFrameAsync(VideoFrame frame)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                var softwareBitmap = frame.SoftwareBitmap;
                //if (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 || softwareBitmap.BitmapAlphaMode == BitmapAlphaMode.Straight)
                //{
                //    softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                //}
                await ScanBitmapAsync(softwareBitmap); ;

                IsBusy = false;
            }
            catch (Exception ex)
            {
                Log.Info(ex);
                IsBusy = false;
            }
        }

        private void CameraPreviewControl_PreviewFailed(object sender, PreviewFailedEventArgs e)
        {
            tbkTip.Text = Constants.GetString("no_camera_found");
        }

        private async Task CleanupCameraAsync()
        {
            UnsubscribeFromEvents();
            if (CameraPreviewControl != null)
            {
                CameraPreviewControl.Stop();
                await CameraPreviewControl.CameraHelper?.CleanUpAsync();
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (CameraPreviewControl != null)
            {
                if (CameraPreviewControl.CameraHelper != null)
                {
                    CameraPreviewControl.CameraHelper.FrameArrived -= CameraPreviewControl_FrameArrived;
                }
                CameraPreviewControl.PreviewFailed -= CameraPreviewControl_PreviewFailed;
            }
        }


        /// <summary>
        /// 解析二维码图片
        /// </summary>
        /// <param name="writeableBmp">图片</param>
        /// <returns></returns>
        private async Task ScanBitmapAsync(SoftwareBitmap bitmap)
        {
            try
            {
                await dispatcherQueue.EnqueueAsync(() =>
                {
                    var result = barcodeReader.Decode(bitmap);
                    if (result != null)
                    {
                        Frame.Navigate(typeof(AuthorizePage), result.Text);
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Info(ex);
                return;
            }
        }

    }
}
