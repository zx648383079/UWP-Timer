using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Threading.Tasks;
using ZoDream.LogTimer.Dialogs;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Services;
using ZoDream.LogTimer.Utils;
using ZoDream.LogTimer.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Plan
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TodayPage : Page
    {
        public TodayPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var auth = App.GetService<IAuthService>();
            fastBtn.Visibility = addBtn.Visibility = auth.Authenticated ? Visibility.Visible : Visibility.Collapsed;
            if (auth.Authenticated)
            {
                ViewModel.Load();
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(IndexPage));
        }

        private void dayBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem as TaskDay;
            if (item == null)
            {
                return;
            }
            Frame.Navigate(typeof(DetailPage), item.Id);
        }

        private void NewButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            splitView.IsPaneOpen = true;
        }

        private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            _ = ViewModel.LoadTask(args.QueryText);
        }

        private void AddToDay_Click(object sender, RoutedEventArgs e)
        {
            _ = ViewModel.AddToDay((sender as Button).DataContext as TaskItem);
        }

        private void ClosePanelBtn_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = false;
        }

        
    }
}
