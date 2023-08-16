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

        private void LargeHeader_Submitted(object sender, TappedRoutedEventArgs e)
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
                _ = App.ViewModel.ShowMessageAsync(Constants.GetString("fk_name_error"));
                return;
            }
            if (string.IsNullOrWhiteSpace(form.Content))
            {
                _ = App.ViewModel.ShowMessageAsync(Constants.GetString("fk_content_error"));
                return;
            }
            _ = save(form);
        }

        private async Task save(Feedback form)
        {
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.Account.SaveFeedbackAsync(form, res => {
                DispatcherQueue.TryEnqueue(() =>
                {
                    _ = App.ViewModel.ShowMessageAsync(res.Message);
                });
            });
            DispatcherQueue.TryEnqueue(() =>
            {
                App.ViewModel.IsLoading = false;
                _ = App.ViewModel.ShowMessageAsync(Constants.GetString("fk_save_success"));
                Frame.GoBack();
            });
        }
    }
}
