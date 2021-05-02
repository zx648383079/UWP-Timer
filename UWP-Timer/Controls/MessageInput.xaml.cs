using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class MessageInput : UserControl
    {
        public MessageInput()
        {
            this.InitializeComponent();
        }



        public MessageInputMode InputMode
        {
            get { return (MessageInputMode)GetValue(InputModeProperty); }
            set { SetValue(InputModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputModeProperty =
            DependencyProperty.Register("InputMode", typeof(MessageInputMode), typeof(MessageInput), new PropertyMetadata(MessageInputMode.NONE, OnModeChange));

        private static void OnModeChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = (d as MessageInput);
            switch (e.OldValue)
            {
                case MessageInputMode.VOICE:
                    instance.RecordBtn.Foreground = new SolidColorBrush(Colors.Black);
                    break;
                case MessageInputMode.EMOJI:
                    instance.EmojiBox.Visibility = Visibility.Collapsed;
                    break;
                case MessageInputMode.MORE:
                    instance.MoreBox.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
            switch (e.NewValue)
            {
                case MessageInputMode.VOICE:
                    instance.RecordBtn.Foreground = new SolidColorBrush(Colors.Red);
                    break;
                case MessageInputMode.EMOJI:
                    instance.EmojiBox.Visibility = Visibility.Visible;
                    break;
                case MessageInputMode.MORE:
                    instance.MoreBox.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void RecordBtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleMode(MessageInputMode.VOICE);
        }

        private void EmojiBtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleMode(MessageInputMode.EMOJI);
        }

        private void MoreBtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleMode(MessageInputMode.MORE);
        }

        public void ToggleMode(MessageInputMode mode, MessageInputMode toggleMode = MessageInputMode.NONE)
        {
            InputMode = InputMode == mode ? toggleMode : mode;
        }
    }

    public enum MessageInputMode
    {
        NONE,
        VOICE,
        EMOJI,
        MORE,
    }
}
