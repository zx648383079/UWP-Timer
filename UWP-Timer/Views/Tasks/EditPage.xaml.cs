using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWP_Timer.Controls;
using UWP_Timer.Models;
using UWP_Timer.Repositories;
using UWP_Timer.Utils;
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
    public sealed partial class EditPage : Page
    {
        public EditPage()
        {
            this.InitializeComponent();
        }

        private int id = 0;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter == null)
            {
                addBtn.Visibility = Visibility.Collapsed;
                return;
            }
            id = (int)e.Parameter;
            if (id < 1)
            {
                addBtn.Visibility = Visibility.Collapsed;
                return;
            }
            _ = LoadTask();
        }

        private async Task LoadTask()
        {
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.Task.GetTaskDetailAsync(id);
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            await dispatcherQueue.EnqueueAsync(() =>
            {
                App.ViewModel.IsLoading = false;
                if (data == null)
                {
                    Frame.GoBack();
                    return;
                }
                titleTb.Title = Constants.GetString("task_edit_title");
                nameTb.Text = data.Name;
                descTb.Text = data.Description;
                timeTb.Text = data.EveryTime.ToString();
            });
        }

        private void LargeHeader_Submited(object sender, TappedRoutedEventArgs e)
        {
            var form = new TaskForm()
            {
                Id = id,
                Name = nameTb.Text,
                Description = descTb.Text,
                EveryTime = Convert.ToInt32(timeTb.Text)
            };
            if (string.IsNullOrWhiteSpace(form.Name))
            {
                _ = new MessageDialog(Constants.GetString("task_name.PlaceholderText")).ShowAsync();
                return;
            }
            _ = save(form);
        }

        private async Task save(TaskForm form)
        {
            App.ViewModel.IsLoading = true;
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            var data = await App.Repository.Task.SaveTaskAsync(form, async res => {
                await dispatcherQueue.EnqueueAsync(() =>
                {
                    _ = new MessageDialog(res.Message).ShowAsync();
                });
            });
            await dispatcherQueue.EnqueueAsync(() =>
            {
                App.ViewModel.IsLoading = false;
                _ = new MessageDialog(Constants.GetString("task_save_success")).ShowAsync();
                Frame.GoBack();
            });
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            _ = showChildAsync();
        }

        private async Task showChildAsync()
        {
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            var dialog = new TaskDialog();
            var result = await dialog.ShowAsync();
            if (result != ContentDialogResult.Primary)
            {
                return;
            }
            if (!dialog.CheckForm())
            {
                return;
            }
            var item = dialog.FormData();
            item.ParentId = id;
            var data = await App.Repository.Task.SaveTaskAsync(item);
            if (data == null)
            {
                return;
            }
            await dispatcherQueue.EnqueueAsync(() =>
            {
                Toast.Tip("添加成功");
            });
        }

        private void shareBtn_Click(object sender, RoutedEventArgs e)
        {
            _ = showShareAsync();
        }

        private async Task showShareAsync()
        {
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            var dialog = new TaskShareDialog();
            var result = await dialog.ShowAsync();
            if (result != ContentDialogResult.Primary)
            {
                return;
            }
            var item = dialog.FormData();
            item.TaskId = id;
            var data = await App.Repository.Task.ShareCreateAsync(item);
            if (data == null)
            {
                return;
            }
            await dispatcherQueue.EnqueueAsync(() =>
            {
                Toast.Tip("分享成功成功");
                // 显示分享链接/二维码
                _ = dialog.ShowAsync();
            });
        }
    }
}
