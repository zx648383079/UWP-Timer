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

namespace UWP_Timer.Views.Account
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CancelPage : Page
    {
        public CancelPage()
        {
            this.InitializeComponent();
        }

        private string Reason;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _ = ShowConfirm();
        }

        private async Task ShowConfirm()
        {
            var dialog = new MessageDialog(Constants.GetString("cancel_confirm_content"), Constants.GetString("cancel_confirm_title"));
            dialog.Commands.Add(new UICommand(Constants.GetString("cancel_confirm_yes"), cmd => { }, 0));
            dialog.Commands.Add(new UICommand(Constants.GetString("cancel_confirm_no"), cmd => { }, 1));
            var result = await dialog.ShowAsync();
            if ((int)result.Id == 1)
            {
                Frame.GoBack();
            }
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Reason))
            {
                _ = new MessageDialog(Constants.GetString("cancel_reason_error")).ShowAsync();
                return;
            }
            _ = cancelAsync(new CancelForm()
            {
                Reason = Reason
            });
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Reason = (sender as RadioButton).Content.ToString();
        }

        private async Task cancelAsync(CancelForm form)
        {
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.Account.CancelUserAsync(form, async res =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    App.ViewModel.IsLoading = false;
                    _ = new MessageDialog(res.Message).ShowAsync();
                });

            });
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                App.ViewModel.IsLoading = false;
                if (data == null)
                {
                    return;
                }
                App.Logout();
                _ = new MessageDialog(Constants.GetString("cancel_success_tip")).ShowAsync();
                Frame.Navigate(typeof(HomePage));
            });
        }
    }
}
