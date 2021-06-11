using Microsoft.Toolkit.Uwp;
using System;
using System.Threading.Tasks;
using UWP_Timer.Controls;
using UWP_Timer.Models;
using UWP_Timer.Utils;
using UWP_Timer.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP_Timer.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        public HomeViewModel ViewModel = new HomeViewModel();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            addBtn.Visibility = App.IsLogin? Visibility.Visible : Visibility.Collapsed;
            if (App.IsLogin)
            {
                ViewModel.Load();
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Tasks.IndexPage));
        }

        private void dayBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem as TaskDay;
            if (item == null)
            {
                return;
            }
            Frame.Navigate(typeof(Tasks.DetailPage), item.Id);
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
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            var dialog = new TaskDialog();
            dialog.Title = "快捷创建任务";
            var result = await dialog.ShowAsync();
            if (result != ContentDialogResult.Primary)
            {
                return;
            }
            if (!dialog.CheckForm())
            {
                return;
            }
            var data = await App.Repository.Task.FastCreateAsync(dialog.FormData());
            if (data == null)
            {
                return;
            }
            await dispatcherQueue.EnqueueAsync(() =>
            {
                Toast.Tip("添加成功");
                ViewModel.Items.Add(data);
            });
        }
    }
}
