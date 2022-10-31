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

namespace ZoDream.LogTimer.Pages.Plan
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
            DispatcherQueue.TryEnqueue(() =>
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
                _ = App.ViewModel.ShowMessageAsync(Constants.GetString("task_name.PlaceholderText"));
                return;
            }
            _ = save(form);
        }

        private async Task save(TaskForm form)
        {
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.Task.SaveTaskAsync(form, res => {
                DispatcherQueue.TryEnqueue(() =>
                {
                    _ = App.ViewModel.ShowMessageAsync(res.Message);
                });
            });
            DispatcherQueue.TryEnqueue(() =>
            {
                App.ViewModel.IsLoading = false;
                _ = App.ViewModel.ShowMessageAsync(Constants.GetString("task_save_success"));
                Frame.GoBack();
            });
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            _ = showChildAsync();
        }

        private async Task showChildAsync()
        {
            //var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            //var dialog = new TaskDialog();
            //var result = await dialog.ShowAsync();
            //if (result != ContentDialogResult.Primary)
            //{
            //    return;
            //}
            //if (!dialog.CheckForm())
            //{
            //    return;
            //}
            //var item = dialog.FormData();
            //item.ParentId = id;
            //var data = await App.Repository.Task.SaveTaskAsync(item);
            //if (data == null)
            //{
            //    return;
            //}
            //await dispatcherQueue.EnqueueAsync(() =>
            //{
            //    Toast.Tip("添加成功");
            //});
        }

        private void shareBtn_Click(object sender, RoutedEventArgs e)
        {
            _ = showShareAsync();
        }

        private async Task showShareAsync()
        {
            //var dialog = new TaskShareDialog();
            //var result = await dialog.ShowAsync();
            //if (result != ContentDialogResult.Primary)
            //{
            //    return;
            //}
            //var item = dialog.FormData();
            //item.TaskId = id;
            //var data = await App.Repository.Task.ShareCreateAsync(item);
            //if (data == null)
            //{
            //    return;
            //}
            //await dispatcherQueue.EnqueueAsync(() =>
            //{
            //    Toast.Tip("分享成功成功");
            //    // 显示分享链接/二维码
            //    _ = dialog.ShowAsync();
            //});
        }
    }
}
