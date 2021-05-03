using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP_Timer.Views.Auth
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class QrPage : Page
    {
        public QrPage()
        {
            this.InitializeComponent();
        }

        private DispatcherQueue dispatcherQueue;
        private string Token = string.Empty;
        private Task timer;
        private QrStatus Status = QrStatus.NONE;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("logo");
            if (imageAnimation != null)
            {
                imageAnimation.TryStart(LogoImg);
            }
            dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            _ = RefreshQrAsync();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async Task RefreshQrAsync()
        {
            var data = await App.Repository.Authorize.QrRefreshAsync();
            if (data == null)
            {
                return;
            }
            await dispatcherQueue.EnqueueAsync(() =>
            {
                Token = data.Token;
                QrImage.Source = Converters.ConverterHelper.ToImg(data.Qr);
                changeStatus(QrStatus.NONE);
                loopCheckQr();
            });
        }

        private void loopCheckQr()
        {
            if(string.IsNullOrEmpty(Token)) {
                return;
            }
            timer = Task.Factory.StartNew(async () =>
            {
                if (string.IsNullOrEmpty(Token))
                {
                    return;
                }
                var data = await App.Repository.Authorize.QrCheckAsync(Token, err =>
                {
                    if (err.Code == 201)
                    {
                        changeStatus(QrStatus.NONE);
                        Thread.Sleep(2000);
                        loopCheckQr();
                        return;
                    }
                    if (err.Code == 202)
                    {
                        changeStatus(QrStatus.CONFIRM);
                        Thread.Sleep(2000);
                        loopCheckQr();
                        return;
                    }
                    if (err.Code == 204)
                    {
                        changeStatus(QrStatus.EXPIRED);
                        return;
                    }
                    changeStatus(QrStatus.REJECT);
                });
                if (data == null)
                {
                    return;
                }
                changeStatus(QrStatus.SUCCESS);
                await dispatcherQueue.EnqueueAsync(() =>
                {
                    timer = null;
                    App.Login(data);
                    Frame.BackStack.Clear();
                    Frame.Navigate(typeof(Member.IndexPage));
                });
            });
        }

        private void changeStatus(QrStatus status)
        {
            Status = status;
            dispatcherQueue.EnqueueAsync(() =>
            {
                switch (status)
                {
                    case QrStatus.CONFIRM:
                        QrBtn.Visibility = Visibility.Visible;
                        IconTb.Glyph = "\xE001";
                        TipTb.Text = "扫描成功，等待确认";
                        break;
                    case QrStatus.EXPIRED:
                        QrBtn.Visibility = Visibility.Visible;
                        IconTb.Glyph = "\xE149";
                        TipTb.Text = "已过期，请刷新";
                        break;
                    case QrStatus.REJECT:
                        QrBtn.Visibility = Visibility.Visible;
                        IconTb.Glyph = "\xE149";
                        TipTb.Text = "登录失败";
                        break;
                    case QrStatus.SUCCESS:
                        QrBtn.Visibility = Visibility.Visible;
                        IconTb.Glyph = "\xE001";
                        TipTb.Text = "登陆成功";
                        break;
                    default:
                        QrBtn.Visibility = Visibility.Collapsed;
                        break;
                }
            });
        }

        private void QrBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Status == QrStatus.EXPIRED || Status == QrStatus.REJECT)
            {
                Status = QrStatus.NONE;
                _ = RefreshQrAsync();
            }
        }
    }

    enum QrStatus
    {
        NONE,
        CONFIRM,
        EXPIRED,
        REJECT,
        SUCCESS,
    }
}
