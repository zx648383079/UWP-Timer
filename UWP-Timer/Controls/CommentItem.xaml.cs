using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Timer.Converters;
using UWP_Timer.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace UWP_Timer.Controls
{
    public sealed partial class CommentItem : UserControl
    {
        public CommentItem()
        {
            this.InitializeComponent();
        }

        public event TypedEventHandler<CommentItem, ActionArgs<CommentBase>> ActionTapped;

        public CommentBase Source
        {
            get { return (CommentBase)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(CommentBase), typeof(CommentItem), new PropertyMetadata(null, new PropertyChangedCallback(OnSourceChange)));

        private static void OnSourceChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CommentItem).RefreshView();
        }



        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(CommentItem), new PropertyMetadata(true, OnOpenChange));

        private static void OnOpenChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CommentItem).RefreshChildrenView();
        }

        private void RefreshChildrenView()
        {
            if (IsOpen)
            {
                ChildrenBox.Visibility = Visibility.Visible;
                ExpandBtn.Text = "收起";
            } else
            {
                ChildrenBox.Visibility = Visibility.Collapsed;
                ExpandBtn.Text = "展开";
            }
        }

        private void RefreshView()
        {
            if (Source == null)
            {
                return;
            }
            TimeTb.Text = ConverterHelper.Ago(Source.CreatedAt);
            if (Source.User != null)
            {
                AvatarImg.ProfilePicture = ConverterHelper.ToImg(Source.User.Avatar);
                UserTb.Text = AvatarImg.DisplayName = Source.User.Name;
            }
            ContentTb.Content = Source.Content;
            ContentTb.Rules = Source.ExtraRule;
            if (Source.Children != null && Source.Children.Count > 0)
            {
                ExpandBtn.Visibility = Visibility.Visible;
                ChildrenBox.Items = Source.Children;
            } else
            {
                ExpandBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void ExpandBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            IsOpen = !IsOpen;
        }

        private void ReplyBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ActionTapped?.Invoke(this, new ActionArgs<CommentBase>(ActionType.REPLY, Source));
        }

        private void AgreeBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ActionTapped?.Invoke(this, new ActionArgs<CommentBase>(ActionType.AGREE, Source));
        }

        private void ReportBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ActionTapped?.Invoke(this, new ActionArgs<CommentBase>(ActionType.REPORT, Source));
        }
    }
}
