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
using ZoDream.LogTimer.Models;
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

        public HomeViewModel ViewModel = new();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            fastBtn.Visibility = addBtn.Visibility = App.Store.Auth.IsAuthenticated ? Visibility.Visible : Visibility.Collapsed;
            if (App.Store.Auth.IsAuthenticated)
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

        private void fastBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _ = FastCreateAsync();
        }

        private async Task FastCreateAsync()
        {
            //var dialog = new TaskDialog();
            //dialog.Title = "快捷创建任务";
            //var result = await dialog.ShowAsync();
            //if (result != ContentDialogResult.Primary)
            //{
            //    return;
            //}
            //if (!dialog.CheckForm())
            //{
            //    return;
            //}
            //var data = await App.Repository.Task.FastCreateAsync(dialog.FormData());
            //if (data == null)
            //{
            //    return;
            //}
            //await dispatcherQueue.EnqueueAsync(() =>
            //{
            //    Toast.Tip("添加成功");
            //    ViewModel.Items.Add(data);
            //});
        }
    }
}
