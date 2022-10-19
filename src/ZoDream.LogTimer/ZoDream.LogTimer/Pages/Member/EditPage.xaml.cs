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

namespace ZoDream.LogTimer.Pages.Member
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditPage : Page
    {
        public EditPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!App.IsLogin || App.ViewModel.User == null)
            {
                Frame.GoBack();
                return;
            }
            var user = App.ViewModel.User;
            nameTb.Text = user.Name;
            sexTb.SelectedIndex = user.Sex;
            birthdayTb.Date = DateTime.Parse(user.Birthday);
            birthdayTb.MaxDate = DateTime.Now;
            birthdayTb.MinDate = DateTime.Now.AddYears(-100);
        }

        private void LargeHeader_Submited(object sender, TappedRoutedEventArgs e)
        {
            var form = new ProfileForm()
            {
                Name = nameTb.Text,
                Sex = sexTb.SelectedIndex,
                Birthday = birthdayTb.Date.Value.ToString("yyyy-MM-dd")
            };
            if (string.IsNullOrWhiteSpace(form.Name))
            {
                _ = new MessageDialog(Constants.GetString("edit_name_error")).ShowAsync();
                return;
            }
            _ = EditProfileAsync(form);
        }

        private async Task EditProfileAsync(ProfileForm form)
        {
            App.ViewModel.IsLoading = true;
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            var data = await App.Repository.User.UpdateAsync(form, async res =>
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
                App.ViewModel.User = data;
                Frame.GoBack();
            });

        }
    }
}