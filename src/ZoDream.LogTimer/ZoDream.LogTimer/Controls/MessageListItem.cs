using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using ZoDream.LogTimer.Converters;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Pages.Member;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    [TemplatePart(Name = InnerPanelName, Type = typeof(StackPanel))]
    public sealed class MessageListItem : Control
    {
        const string InnerPanelName = "PART_InnerPanel";
        public MessageListItem()
        {
            this.DefaultStyleKey = typeof(MessageListItem);
            SizeChanged += MessageListItem_SizeChanged;
        }

        private void MessageListItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (InnerPanel != null)
            {
                InnerPanel.MaxWidth = e.NewSize.Width - 100;
            }
        }

        private StackPanel InnerPanel;

        public ImageSource Avatar
        {
            get { return (ImageSource)GetValue(AvatarProperty); }
            set { SetValue(AvatarProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Avatar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvatarProperty =
            DependencyProperty.Register("Avatar", typeof(ImageSource), typeof(MessageListItem), new PropertyMetadata(null));



        public string Nickname
        {
            get { return (string)GetValue(NicknameProperty); }
            set { SetValue(NicknameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Nickname.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NicknameProperty =
            DependencyProperty.Register("Nickname", typeof(string), typeof(MessageListItem), new PropertyMetadata(default(string)));

        public MessageBase Source
        {
            get { return (MessageBase)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(MessageBase), typeof(MessageListItem), new PropertyMetadata(null, OnSourceChange));

        private static void OnSourceChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MessageListItem).RefreshView();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InnerPanel = GetTemplateChild(InnerPanelName) as StackPanel;
            RefreshView();
        }

        private void RefreshView()
        {
            if (Source == null || InnerPanel == null)
            {
                return;
            }
            InnerPanel.Children.Clear();
            Avatar = ConverterHelper.ToImg(Source.User.Avatar);
            Nickname = Source.User.Name;
            switch (Source.Type)
            {
                case MessageType.IMAGE:
                    break;
                case MessageType.VIDEO:
                    break;
                case MessageType.VOICE:
                    break;
                case MessageType.FILE:
                    break;
                default:
                    InnerPanel.Children.Add(new RuleBlock()
                    {
                        Content = Source.Content,
                        Rules = Source.ExtraRule
                    });
                    break;
            }
        }
    }
}
