using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using ZoDream.LogTimer.Extensions;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Utils;
using ZoDream.LogTimer.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Micro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
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
            }
            else
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

        private void CommentListBox_ActionTapped(Controls.CommentListItem sender, ActionArgs<CommentBase> args)
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
