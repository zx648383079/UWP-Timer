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

namespace UWP_Timer.Views.Member
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
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
            if (!App.IsLogin() || App.ViewModel.User == null)
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
