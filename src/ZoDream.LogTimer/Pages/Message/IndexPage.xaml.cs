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
using Windows.Foundation;
using Windows.Foundation.Collections;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Services;
using ZoDream.LogTimer.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Message
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IndexPage : Page
    {
        public IndexPage()
        {
            this.InitializeComponent();
        }
        public BulletinViewModel ViewModel = new BulletinViewModel();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var auth = App.GetService<IAuthService>();
            if (!auth.Authenticated)
            {
                return;
            }
            MessageBox.Sender = auth.AuthenticatedUser!.Id;
        }

        private void commentBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadType(8);
            InputBox.Visibility = Visibility.Collapsed;
        }

        private void atBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadType(7);
            InputBox.Visibility = Visibility.Collapsed;
        }

        private void agreeBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadType(6);
            InputBox.Visibility = Visibility.Collapsed;
        }

        private void systemBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadUser(0);
            InputBox.Visibility = Visibility.Collapsed;
        }

        private void userList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var user = ((sender as ListView).SelectedItem as UserItem).Id;
            ViewModel.LoadUser(user);
            if (user < 1)
            {
                InputBox.Visibility = Visibility.Collapsed;
                return;
            }
            InputBox.Visibility = Visibility.Visible;
        }
    }
}
