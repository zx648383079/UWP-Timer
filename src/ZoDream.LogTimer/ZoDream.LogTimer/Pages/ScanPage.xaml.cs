﻿using Microsoft.UI.Xaml;
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
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.System;
using Windows.UI.Core;
using ZoDream.LogTimer.Extensions;
using ZoDream.LogTimer.Utils;
using ZXing;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScanPage : Page
    {
        BarcodeReader<SoftwareBitmap> barcodeReader;
        bool IsBusy = false;
        DispatcherQueue dispatcherQueue;

        public ScanPage()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            //Application.Current.Suspending += Application_Suspending;
            //Application.Current.Resuming += Application_Resuming;
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

        //private async void Application_Suspending(object sender, SuspendingEventArgs e)
        //{
        //    // Handle global application events only if this page is active
        //    if (Frame.CurrentSourcePageType == typeof(MainWindow))
        //    {
        //        var deferral = e.SuspendingOperation.GetDeferral();

        //        await CleanupCameraAsync();

        //        deferral.Complete();
        //    }

        //}

        //private void Application_Resuming(object sender, object o)
        //{
        //    // Handle global application events only if this page is active
        //    if (Frame.CurrentSourcePageType == typeof(MainWindow))
        //    {
        //    }
        //}

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            //await CleanupCameraAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _ = InitCameraAsync();
        }

        private async Task InitCameraAsync()
        {
            // UnsubscribeFromEvents();
            dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            if (CameraPreviewControl != null)
            {
                //CameraPreviewControl.PreviewFailed += CameraPreviewControl_PreviewFailed;
                //await CameraPreviewControl.StartAsync();
                //CameraPreviewControl.CameraHelper.FrameArrived += CameraPreviewControl_FrameArrived;
                //barcodeReader = new BarcodeReader();
            }

        }

        //private void CameraPreviewControl_FrameArrived(object sender, Microsoft.Toolkit.Uwp.Helpers.FrameEventArgs e)
        //{
        //    _ = SanFrameAsync(e.VideoFrame);
        //}

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

        //private void CameraPreviewControl_PreviewFailed(object sender, PreviewFailedEventArgs e)
        //{
        //    tbkTip.Text = Constants.GetString("no_camera_found");
        //}

        //private async Task CleanupCameraAsync()
        //{
        //    UnsubscribeFromEvents();
        //    if (CameraPreviewControl != null)
        //    {
        //        CameraPreviewControl.Stop();
        //        await CameraPreviewControl.CameraHelper?.CleanUpAsync();
        //    }
        //}

        //private void UnsubscribeFromEvents()
        //{
        //    if (CameraPreviewControl != null)
        //    {
        //        if (CameraPreviewControl.CameraHelper != null)
        //        {
        //            CameraPreviewControl.CameraHelper.FrameArrived -= CameraPreviewControl_FrameArrived;
        //        }
        //        CameraPreviewControl.PreviewFailed -= CameraPreviewControl_PreviewFailed;
        //    }
        //}


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
