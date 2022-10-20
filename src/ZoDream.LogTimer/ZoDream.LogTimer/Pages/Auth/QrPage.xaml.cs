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
            DispatcherQueue.TryEnqueue(() =>
            {
                Token = data.Token;
                QrImage.Source = Converters.ConverterHelper.ToImg(data.Qr);
                changeStatus(QrStatus.NONE);
                loopCheckQr();
            });
        }

        private void loopCheckQr()
        {
            if (string.IsNullOrEmpty(Token))
            {
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
                DispatcherQueue.TryEnqueue(() =>
                {
                    timer = null;
                    App.Store.Auth.LoginAsync(data.Token, data);
                    Frame.BackStack.Clear();
                    Frame.Navigate(typeof(Member.IndexPage));
                });
            });
        }

        private void changeStatus(QrStatus status)
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
