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
using UWP_Timer.ViewModels;
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

namespace UWP_Timer.Views.Tasks
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class IndexPage : Page
    {
        public IndexPage()
        {
            this.InitializeComponent();
        }

        public TaskViewModel ViewModel = new TaskViewModel();

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
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            await dispatcherQueue.EnqueueAsync(() =>
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
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            await dispatcherQueue.EnqueueAsync(() =>
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
