using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWP_Timer.Models;
using UWP_Timer.Utils;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
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
            Unloaded += MessageInput_Unloaded;
        }

        private void MessageInput_Unloaded(object sender, RoutedEventArgs e)
        {
            Recorder.Dispose();
        }

        private AudioRecorder Recorder = new AudioRecorder();
        public event TypedEventHandler<MessageInput, MessageInputArgs> Confirm;

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
                    _ = instance.StopRecordAsync();
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
                    _ = instance.StartRecordAsync();
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

        private async Task StartRecordAsync()
        {
            Recorder.Dispose();
            RecordBtn.Foreground = new SolidColorBrush(Colors.Red);
            await Recorder.StartAsync();
        }

        private async Task StopRecordAsync()
        {
            RecordBtn.Foreground = new SolidColorBrush(Colors.Black);
            if (!Recorder.IsRecording)
            {
                return;
            }
            await Recorder.StopAsync();
            Confirm?.Invoke(this, new MessageInputArgs(MessageType.VOICE, Recorder.Stream));
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

        private void EmojiBox_SelectionChanged(EmojiBox sender, EmojiTappedArgs args)
        {
            ContentTb.SelectedText = args.Emoji.Type > 0 ? args.Emoji.Content : $"[{args.Emoji.Name}]";
        }

        private void ContentTb_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Confirm?.Invoke(this, new MessageInputArgs(ContentTb.Text));
                ContentTb.Text = "";
            }
        }

        private void ImageBtn_Click(object sender, RoutedEventArgs e)
        {
            _ = ImageUpload();
        }

        private async Task ImageUpload()
        {
            var filePicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                FileTypeFilter =
                {
                    ".png", ".jpg", ".jpeg"
                }
            };
            var file = await filePicker.PickSingleFileAsync();
            if (file == null)
            {
                return;
            }
            Confirm?.Invoke(this, new MessageInputArgs(MessageType.IMAGE, file));
        }

        private void VideoBtn_Click(object sender, RoutedEventArgs e)
        {
            _ = VideoUpload();   
        }

        private async Task VideoUpload()
        {
            var filePicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.VideosLibrary,
                FileTypeFilter =
                {
                    ".mp4", ".mkv", ".avi"
                }
            };
            var file = await filePicker.PickSingleFileAsync();
            if (file == null)
            {
                return;
            }
            Confirm?.Invoke(this, new MessageInputArgs(MessageType.VIDEO, file));
        }

        private void FileBtn_Click(object sender, RoutedEventArgs e)
        {
            _ = FileUpload();
        }

        private async Task FileUpload()
        {
            var filePicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };
            var file = await filePicker.PickSingleFileAsync();
            if (file == null)
            {
                return;
            }
            Confirm?.Invoke(this, new MessageInputArgs(MessageType.FILE, file));
        }
    }

    public class MessageInputArgs
    {
        public readonly MessageType Type;

        public readonly object Data;

        public MessageInputArgs()
        {
            Type = MessageType.TEXT;
            Data = "";
        }

        public MessageInputArgs(string data)
        {
            Type = MessageType.TEXT;
            Data = data;
        }

        public MessageInputArgs(MessageType type, object data)
        {
            Type = type;
            Data = data;
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
