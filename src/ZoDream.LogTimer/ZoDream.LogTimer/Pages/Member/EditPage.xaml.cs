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
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;

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
            if (!App.Store.Auth.IsAuthenticated || App.Store.Auth.User is null)
            {
                Frame.GoBack();
                return;
            }
            var user = App.Store.Auth.User;
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
                _ = App.ViewModel.ShowMessageAsync(Constants.GetString("edit_name_error"));
                return;
            }
            _ = EditProfileAsync(form);
        }

        private async Task EditProfileAsync(ProfileForm form)
        {
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.User.UpdateAsync(form, res =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    App.ViewModel.IsLoading = false;
                    _ = App.ViewModel.ShowMessageAsync(res.Message);
                });

            });
            DispatcherQueue.TryEnqueue(() =>
            {
                App.ViewModel.IsLoading = false;
                if (data == null)
                {
                    return;
                }
                App.Store.Auth.LoginAsync(data);
                Frame.GoBack();
            });

        }
    }
}
