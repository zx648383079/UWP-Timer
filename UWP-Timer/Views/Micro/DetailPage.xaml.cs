﻿using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWP_Timer.Converters;
using UWP_Timer.Models;
using UWP_Timer.Utils;
using UWP_Timer.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP_Timer.Views.Micro
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        public DetailPage()
        {
            this.InitializeComponent();
        }

        public MicroDetailViewModel ViewModel = new MicroDetailViewModel();
        private int ParentId = 0;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var item = (MicroItem)e.Parameter;
            if (string.IsNullOrEmpty(item.Content))
            {
                _ = LoadDetailAsync(item.Id);
            } else
            {
                SetMicro(item);
            }
            ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("micro");
            if (imageAnimation != null)
            {
                imageAnimation.TryStart(MicroView);
            }
            if (App.IsLogin)
            {
                ViewModel.User = App.ViewModel.User;
            }
        }

        private async Task LoadDetailAsync(int id)
        {
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.Micro.GetAsync(id);
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            await dispatcherQueue.EnqueueAsync(() =>
            {
                App.ViewModel.IsLoading = false;
                if (data == null)
                {
                    Frame.GoBack();
                    return;
                }
                SetMicro(data);
            });
        }

        private void SetMicro(MicroItem data)
        {
            ViewModel.Data = data;
            _ = ViewModel.TapRefreshAsync();
        }

        private void EmojiBox_SelectionChanged(Controls.EmojiBox sender, Controls.EmojiTappedArgs args)
        {
            CommentTb.SelectedText = args.Emoji.Type > 0 ? args.Emoji.Content : $"[{args.Emoji.Name}]";
            emojiFlyout.Hide();
        }

        private void CommentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CommentTb.Text))
            {
                Toast.Tip("请输入内容");
                return;
            }
            CommentBtn.IsEnabled = false;
            _ = CreateAsync(new MicroCommentForm()
            {
                MicroId = ViewModel.Data.Id,
                ParentId = ParentId,
                Conent = CommentTb.Text,
                IsForward = (bool)ForwardCheck.IsChecked
            });
        }

        private async Task CreateAsync(MicroCommentForm form)
        {
            var data = await App.Repository.Micro.CreateCommentAsync(form);
            if (data == null)
            {
                return;
            }
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            await dispatcherQueue.EnqueueAsync(() =>
            {
                Toast.Tip("评论成功");
                CommentTb.Text = "";
                ParentId = 0;
            });
        }

        private void CommentListBox_ActionTapped(Controls.CommentItem sender, ActionArgs<CommentBase> args)
        {
            if (args.Action == ActionType.REPLY)
            {
                ParentId = args.Data.Id;
                if (args.Data.User != null)
                {
                    CommentTb.Text += $"@{args.Data.User.Name} ";
                }
            }
        }
    }
}
