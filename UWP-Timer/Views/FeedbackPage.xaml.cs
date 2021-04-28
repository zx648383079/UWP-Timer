using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWP_Timer.Models;
using UWP_Timer.Repositories;
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
    public sealed partial class FeedbackPage : Page
    {
        public FeedbackPage()
        {
            this.InitializeComponent();
        }

        private void LargeHeader_Submited(object sender, TappedRoutedEventArgs e)
        {
            var form = new Feedback()
            {
                Email = emailTb.Text,
                Name = nameTb.Text,
                Phone = phoneTb.Text,
                Content = contentTb.Text
            };
            if (string.IsNullOrWhiteSpace(form.Name))
            {
                _ = new MessageDialog(Constants.GetString("fk_name_error")).ShowAsync();
                return;
            }
            if (string.IsNullOrWhiteSpace(form.Content))
            {
                _ = new MessageDialog(Constants.GetString("fk_content_error")).ShowAsync();
                return;
            }
            _ = save(form);
        }

        private async Task save(Feedback form)
        {
            App.ViewModel.IsLoading = true;
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            var data = await App.Repository.Account.SaveFeedbackAsync(form, async res => {
                await dispatcherQueue.EnqueueAsync(() =>
                {
                    _ = new MessageDialog(res.Message).ShowAsync();
                });
            });
            await dispatcherQueue.EnqueueAsync(() =>
            {
                App.ViewModel.IsLoading = false;
                _ = new MessageDialog(Constants.GetString("fk_save_success")).ShowAsync();
                Frame.GoBack();
            });
        }
    }
}
