using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using ZoDream.LogTimer.Converters;
using ZoDream.LogTimer.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    [TemplatePart(Name = InnerBlockName, Type = typeof(RuleBlock))]
    [TemplatePart(Name = ExpandBtnName, Type = typeof(TextBlock))]
    [TemplatePart(Name = ReportBtnName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = AgreeBtnName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ReplyBtnName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ChildrenPanelName, Type = typeof(CommentListBox))]
    public sealed class CommentListItem : Control
    {
        const string InnerBlockName = "PART_InnerBlock";
        const string ExpandBtnName = "PART_ExpandBtn";
        const string ReportBtnName = "PART_ReportBtn";
        const string AgreeBtnName = "PART_AgreeBtn";
        const string ReplyBtnName = "PART_ReplyBtn";
        const string ChildrenPanelName = "PART_ChildrenPanel";

        public CommentListItem()
        {
            this.DefaultStyleKey = typeof(CommentListItem);
        }

        public event TypedEventHandler<CommentListItem, ActionArgs<CommentBase>> ActionTapped;
        private RuleBlock InnerBlock;
        private TextBlock ExpandBtn;
        private FrameworkElement ReportBtn;
        private FrameworkElement AgreeBtn;
        private FrameworkElement ReplyBtn;
        private CommentListBox ChildrenPanel;

        public CommentBase Source
        {
            get { return (CommentBase)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(CommentBase), typeof(CommentListItem), new PropertyMetadata(null, new PropertyChangedCallback(OnSourceChange)));

        private static void OnSourceChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CommentListItem).RefreshView();
        }



        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(CommentListItem), new PropertyMetadata(true, OnOpenChange));

        private static void OnOpenChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CommentListItem).RefreshChildrenView();
        }
        public ImageSource Avatar
        {
            get { return (ImageSource)GetValue(AvatarProperty); }
            set { SetValue(AvatarProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Avatar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvatarProperty =
            DependencyProperty.Register("Avatar", typeof(ImageSource), typeof(CommentListItem), new PropertyMetadata(null));



        public string Nickname
        {
            get { return (string)GetValue(NicknameProperty); }
            set { SetValue(NicknameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Nickname.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NicknameProperty =
            DependencyProperty.Register("Nickname", typeof(string), typeof(CommentListItem), new PropertyMetadata(default(string)));

        public string CreatedAt
        {
            get { return (string)GetValue(CreatedAtProperty); }
            set { SetValue(CreatedAtProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CreatedAt.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CreatedAtProperty =
            DependencyProperty.Register("CreatedAt", typeof(string), typeof(CommentListItem), new PropertyMetadata(default(string)));


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InnerBlock = GetTemplateChild(InnerBlockName) as RuleBlock;
            ExpandBtn = GetTemplateChild(ExpandBtnName) as TextBlock;
            ReportBtn = GetTemplateChild(ReportBtnName) as FrameworkElement;
            AgreeBtn = GetTemplateChild(AgreeBtnName) as FrameworkElement;
            ReplyBtn = GetTemplateChild(ReplyBtnName) as FrameworkElement;
            ChildrenPanel = GetTemplateChild(ChildrenPanelName) as CommentListBox;
            if (ExpandBtn != null)
            {
                ExpandBtn.Tapped += ExpandBtn_Tapped;
            }
            if (ReplyBtn != null)
            {
                ReplyBtn.Tapped += ReplyBtn_Tapped;
            }
            if (AgreeBtn != null)
            {
                AgreeBtn.Tapped += AgreeBtn_Tapped;
            }
            if (ReportBtn != null) 
            {
                ReportBtn.Tapped += ReportBtn_Tapped;
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

        private void RefreshChildrenView()
        {
            if (IsOpen)
            {
                ChildrenPanel.Visibility = Visibility.Visible;
                ExpandBtn.Text = "收起";
            }
            else
            {
                ChildrenPanel.Visibility = Visibility.Collapsed;
                ExpandBtn.Text = "展开";
            }
        }

        private void RefreshView()
        {
            if (Source == null)
            {
                return;
            }
            CreatedAt = ConverterHelper.Ago(Source.CreatedAt);
            if (Source.User != null)
            {
                Avatar = ConverterHelper.ToImg(Source.User.Avatar);
                Nickname = Source.User.Name;
            }
            InnerBlock.Content = Source.Content;
            InnerBlock.Rules = Source.ExtraRule;
            if (Source.Children != null && Source.Children.Count > 0)
            {
                ExpandBtn.Visibility = Visibility.Visible;
                ChildrenPanel.Items = Source.Children;
            }
            else
            {
                ExpandBtn.Visibility = Visibility.Collapsed;
            }
        }
    }
}
