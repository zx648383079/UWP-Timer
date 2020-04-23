﻿using Microsoft.Toolkit.Uwp.Helpers;
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
                return;
            }
            id = (int)e.Parameter;
            if (id < 1)
            {
                return;
            }
            _ = LoadTask();
        }

        private async Task LoadTask()
        {
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.Task.GetTaskDetailAsync(id);
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
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
            var data = await App.Repository.Task.SaveTaskAsync(form, async res => {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    _ = new MessageDialog(res.Message).ShowAsync();
                });
            });
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                App.ViewModel.IsLoading = false;
                _ = new MessageDialog(Constants.GetString("task_save_success")).ShowAsync();
                Frame.GoBack();
            });
        }
    }
}