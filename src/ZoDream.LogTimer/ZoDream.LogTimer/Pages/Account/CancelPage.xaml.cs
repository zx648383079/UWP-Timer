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
using Windows.UI.Popups;
using ZoDream.LogTimer.Extensions;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Account
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
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
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            var data = await App.Repository.Account.CancelUserAsync(form, async res =>
            {
                await dispatcherQueue.EnqueueAsync(() =>
                {
                    App.ViewModel.IsLoading = false;
                    _ = new MessageDialog(res.Message).ShowAsync();
                });

            });
            await dispatcherQueue.EnqueueAsync(() =>
            {
                App.ViewModel.IsLoading = false;
                if (data == null)
                {
                    return;
                }
                App.Logout();
                _ = new MessageDialog(Constants.GetString("cancel_success_tip")).ShowAsync();
                Frame.Navigate(typeof(Plan.TodayPage));
            });
        }
    }
}