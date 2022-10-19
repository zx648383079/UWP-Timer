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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using ZoDream.LogTimer.Extensions;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
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
