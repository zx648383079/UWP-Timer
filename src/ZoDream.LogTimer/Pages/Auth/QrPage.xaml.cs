using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Auth
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QrPage : Page
    {
        public QrPage()
        {
            this.InitializeComponent();
        }

        private string Token = string.Empty;
        private CancellationTokenSource tokenSource;
        private QrStatus Status = QrStatus.NONE;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("logo");
            imageAnimation?.TryStart(LogoImg);
            tokenSource = new CancellationTokenSource();
            _ = RefreshQrAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            tokenSource.Cancel();
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
            DispatcherQueue.TryEnqueue(() =>
            {
                Token = data.Token;
                QrImage.Source = Converters.ConverterHelper.ToImg(data.Qr);
                ChangeStatus(QrStatus.NONE);
                LoopCheckQr();
            });
        }

        private void LoopCheckQr()
        {
            if (string.IsNullOrEmpty(Token))
            {
                return;
            }
            var token = tokenSource.Token;
            Task.Factory.StartNew(async () =>
            {
                if (string.IsNullOrEmpty(Token) || token.IsCancellationRequested)
                {
                    return;
                }
                var data = await App.Repository.Authorize.QrCheckAsync(Token, err =>
                {
                    if (err.Code == 201)
                    {
                        ChangeStatus(QrStatus.NONE);
                        Thread.Sleep(2000);
                        LoopCheckQr();
                        return;
                    }
                    if (err.Code == 202)
                    {
                        ChangeStatus(QrStatus.CONFIRM);
                        Thread.Sleep(2000);
                        LoopCheckQr();
                        return;
                    }
                    if (err.Code == 204)
                    {
                        ChangeStatus(QrStatus.EXPIRED);
                        return;
                    }
                    ChangeStatus(QrStatus.REJECT);
                });
                if (data == null)
                {
                    return;
                }
                ChangeStatus(QrStatus.SUCCESS);
                DispatcherQueue.TryEnqueue(() =>
                {
                    App.Store.Auth.LoginAsync(data.Token, data);
                    Frame.BackStack.Clear();
                    Frame.Navigate(typeof(Member.IndexPage));
                });
            }, token);
        }

        private void ChangeStatus(QrStatus status)
        {
            Status = status;
            DispatcherQueue.TryEnqueue(() =>
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
