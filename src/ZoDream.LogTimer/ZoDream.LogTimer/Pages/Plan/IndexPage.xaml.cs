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
using ZoDream.LogTimer.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Plan
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

        public TaskViewModel ViewModel = new();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            addBtn.Visibility = App.Store.Auth.IsAuthenticated ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var label = (sender as AppBarButton).Name;
            if (label == "addBtn")
            {
                Frame.Navigate(typeof(EditPage));
                return;
            }
            if (label == "enterEditBtn")
            {
                ViewModel.InEdit = true;
                taskBox.SelectionMode = ListViewSelectionMode.Multiple;
                return;
            }
            if (label == "cancelMode")
            {
                ViewModel.InEdit = false;
                taskBox.SelectedIndex = -1;
                taskBox.SelectionMode = ListViewSelectionMode.Single;
                return;
            }
            if (taskBox.SelectedItems.Count < 1)
            {
                _ = new MessageDialog(Constants.GetString("task_selected_error")).ShowAsync();
                return;
            }
            var items = new int[taskBox.SelectedItems.Count];
            for (int i = 0; i < taskBox.SelectedItems.Count; i++)
            {
                items[i] = (taskBox.SelectedItems[i] as TaskItem).Id;
            }
            if (label == "stopTask")
            {
                _ = stopTaskAsync(items);
                return;
            }
            _ = addTaskAsync(items);
        }

        private void taskBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.InEdit)
            {
                return;
            }
            var item = (sender as ListView).SelectedItem as TaskItem;
            if (item == null)
            {
                return;
            }
            Frame.Navigate(typeof(EditPage), item.Id);
        }

        private void statusBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var status = (int)(statusBar.SelectedItem as TabItem).Value;
            ViewModel.Load(status);
            ViewModel.InEdit = false;
            taskBox.SelectedIndex = -1;
            taskBox.SelectionMode = ListViewSelectionMode.Single;
            enterEditBtn.Visibility = status < 2 ? Visibility.Visible : Visibility.Collapsed;
        }

        private async Task stopTaskAsync(int[] items)
        {
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.Task.BatchStopTaskAsync(items);
            DispatcherQueue.TryEnqueue(() =>
            {
                App.ViewModel.IsLoading = false;
                if (data == null)
                {
                    return;
                }
                _ = new MessageDialog(Constants.GetString("task_stop_success")).ShowAsync();
                ViewModel.Refresh();
            });
        }

        private async Task addTaskAsync(int[] items)
        {
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.Task.BatchAddTaskAsync(items);
            DispatcherQueue.TryEnqueue(() =>
            {
                App.ViewModel.IsLoading = false;
                if (data == null)
                {
                    return;
                }
                _ = new MessageDialog(Constants.GetString("task_add_today_success")).ShowAsync();
            });
        }
    }
}
