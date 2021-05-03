using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class MessageItem : UserControl
    {
        public MessageItem()
        {
            this.InitializeComponent();
            SizeChanged += MessageItem_SizeChanged;
        }

        private void MessageItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentBox.MaxWidth = e.NewSize.Width - 100;
        }

        public MessageBase Source
        {
            get { return (MessageBase)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(MessageBase), typeof(MessageItem), new PropertyMetadata(null, OnSourceChange));

        private static void OnSourceChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MessageItem).RefreshView();
        }

        private void RefreshView()
        {
            if (Source == null)
            {
                return;
            }
            ContentBox.Children.Clear();
            AvatarImage.ProfilePicture = Converters.ConverterHelper.ToImg(Source.User.Avatar);
            AvatarImage.DisplayName = Source.User.Name;
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
                    ContentBox.Children.Add(new RuleBlock()
                    {
                        Content = Source.Content,
                        Rules = Source.ExtraRule
                    });
                    break;
            }
        }
    }
}
