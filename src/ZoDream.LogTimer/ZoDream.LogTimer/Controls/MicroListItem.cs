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
    [TemplatePart(Name = MediaPanelName, Type = typeof(MediaViewer))]
    [TemplatePart(Name = CollectBtnName, Type = typeof(Button))]
    [TemplatePart(Name = ForwardBtnName, Type = typeof(Button))]
    [TemplatePart(Name = CommentBtnName, Type = typeof(Button))]
    [TemplatePart(Name = LikeBtnName, Type = typeof(Button))]
    public sealed class MicroListItem : Control
    {
        const string InnerBlockName = "PART_InnerBlock";
        const string MediaPanelName = "PART_MediaPanel";
        const string CollectBtnName = "PART_CollectBtn";
        const string ForwardBtnName = "PART_ForwardBtn";
        const string CommentBtnName = "PART_CommentBtn";
        const string LikeBtnName = "PART_LikeBtn";
        public MicroListItem()
        {
            this.DefaultStyleKey = typeof(MicroListItem);
        }

        public event TypedEventHandler<MicroListItem, ActionArgs<MicroItem>> ActionTapped;
        public event TypedEventHandler<MicroListItem, RuleTappedArgs> RuleTapped;
        private RuleBlock InnerBlock;
        private Button CollectBtn;
        private Button ForwardBtn;
        private Button CommentBtn;
        private Button LikeBtn;
        private MediaViewer MediaPanel;

        public MicroItem Source
        {
            get { return (MicroItem)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(MicroItem), typeof(MicroListItem), new PropertyMetadata(null, OnSourceChange));
        public string Avatar
        {
            get { return (string)GetValue(AvatarProperty); }
            set { SetValue(AvatarProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Avatar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvatarProperty =
            DependencyProperty.Register("Avatar", typeof(string), typeof(MicroListItem), new PropertyMetadata(default(string)));



        public string Nickname
        {
            get { return (string)GetValue(NicknameProperty); }
            set { SetValue(NicknameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Nickname.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NicknameProperty =
            DependencyProperty.Register("Nickname", typeof(string), typeof(MicroListItem), new PropertyMetadata(default(string)));

        public string CreatedAt
        {
            get { return (string)GetValue(CreatedAtProperty); }
            set { SetValue(CreatedAtProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CreatedAt.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CreatedAtProperty =
            DependencyProperty.Register("CreatedAt", typeof(string), typeof(MicroListItem), new PropertyMetadata(default(string)));


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InnerBlock = GetTemplateChild(InnerBlockName) as RuleBlock;
            CollectBtn = GetTemplateChild(CollectBtnName) as Button;
            LikeBtn = GetTemplateChild(LikeBtnName) as Button;
            ForwardBtn = GetTemplateChild(ForwardBtnName) as Button;
            CommentBtn = GetTemplateChild(CommentBtnName) as Button;
            MediaPanel = GetTemplateChild(MediaPanelName) as MediaViewer;
            if (CollectBtn!= null)
            {
                CollectBtn.Click += CollectBtn_Click;
            }
            if (LikeBtn!= null)
            {
                LikeBtn.Click += LikeBtn_Tapped;
            }
            if (ForwardBtn != null)
            {
                ForwardBtn.Click += ForwardBtn_Tapped;
            }
            if (CommentBtn != null)
            {
                CommentBtn.Click += CommentBtn_Tapped;
            }
            if (InnerBlock != null)
            {
                InnerBlock.RuleTapped += InnerBlock_RuleTapped;
            }
            RefreshView();
        }

        private static void OnSourceChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MicroListItem).RefreshView();
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
                Avatar = Source.User.Avatar;
                Nickname = Source.User.Name;
            }
            if (InnerBlock == null)
            {
                return;
            }
            InnerBlock.Content = Source.Content;
            InnerBlock.Rules = Source.ExtraRule;
            MediaPanel.Items = Source.Attachment;
        }

        private void ForwardBtn_Tapped(object sender, RoutedEventArgs e)
        {
            ActionTapped?.Invoke(this, new ActionArgs<MicroItem>(ActionType.FORWARD, Source));
        }

        private void CommentBtn_Tapped(object sender, RoutedEventArgs e)
        {
            ActionTapped?.Invoke(this, new ActionArgs<MicroItem>(ActionType.COMMENT, Source));
        }

        private void LikeBtn_Tapped(object sender, RoutedEventArgs e)
        {
            ActionTapped?.Invoke(this, new ActionArgs<MicroItem>(ActionType.Like, Source));
        }

        private void InnerBlock_RuleTapped(RuleBlock sender, RuleTappedArgs args)
        {
            RuleTapped?.Invoke(this, args);
        }

        private void CollectBtn_Click(object sender, RoutedEventArgs e)
        {
            ActionTapped?.Invoke(this, new ActionArgs<MicroItem>(ActionType.COLLECT, Source));
        }
    }
}
