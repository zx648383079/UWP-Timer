using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using Windows.Storage.Pickers;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    [TemplatePart(Name = RecordBtnName, Type = typeof(Button))]
    [TemplatePart(Name = EmojiBtnName, Type = typeof(Button))]
    [TemplatePart(Name = MoreBtnName, Type = typeof(Button))]
    [TemplatePart(Name = EmojiPanelName, Type = typeof(EmojiBox))]
    [TemplatePart(Name = MorePanelName, Type = typeof(GridView))]
    [TemplatePart(Name = InnerTbName, Type = typeof(TextBox))]
    public sealed class MessageInput : Control
    {
        const string RecordBtnName = "PART_RecordBtn";
        const string EmojiBtnName = "PART_EmojiBtn";
        const string MoreBtnName = "PART_MoreBtn";
        const string EmojiPanelName = "PART_EmojiPanel";
        const string MorePanelName = "PART_MorePanel";
        const string InnerTbName = "PART_InnerTb";
        public MessageInput()
        {
            this.DefaultStyleKey = typeof(MessageInput);
            Unloaded += MessageInput_Unloaded;
        }

        private void MessageInput_Unloaded(object sender, RoutedEventArgs e)
        {
            Recorder.Dispose();
        }
        private readonly AudioRecorder Recorder = new();
        public event TypedEventHandler<MessageInput, MessageInputArgs> Confirm;
        private Button RecordBtn;
        private Button EmojiBtn;
        private Button MoreBtn;
        private EmojiBox EmojiPanel;
        private GridView MorePanel;
        private TextBox InnerTb;




        public IList<MessageMoreItem> MoreItems
        {
            get { return (IList<MessageMoreItem>)GetValue(MoreItemsProperty); }
            set { SetValue(MoreItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MoreItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoreItemsProperty =
            DependencyProperty.Register("MoreItems", typeof(IList<MessageMoreItem>), typeof(MessageInput), 
                new PropertyMetadata(new List<MessageMoreItem>()
                {
                    new MessageMoreItem("图片", "\uE158"),
                    new MessageMoreItem("视频", "\uE116"),
                    new MessageMoreItem("文件", "\uE1A5")
                }));



        public Visibility EmojiVisibility
        {
            get { return (Visibility)GetValue(EmojiVisibilityProperty); }
            set { SetValue(EmojiVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EmojiVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmojiVisibilityProperty =
            DependencyProperty.Register("EmojiVisibility", typeof(Visibility), typeof(MessageInput), new PropertyMetadata(Visibility.Collapsed));



        public Visibility MoreVisibility
        {
            get { return (Visibility)GetValue(MoreVisibilityProperty); }
            set { SetValue(MoreVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MoreVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoreVisibilityProperty =
            DependencyProperty.Register("MoreVisibility", typeof(Visibility), typeof(MessageInput), new PropertyMetadata(Visibility.Collapsed));



        public MessageInputMode InputMode
        {
            get { return (MessageInputMode)GetValue(InputModeProperty); }
            set { SetValue(InputModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputModeProperty =
            DependencyProperty.Register("InputMode", typeof(MessageInputMode), typeof(MessageInput), new PropertyMetadata(MessageInputMode.NONE, OnModeChange));

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            RecordBtn = GetTemplateChild(RecordBtnName) as Button;
            EmojiBtn = GetTemplateChild(EmojiBtnName) as Button;
            MoreBtn = GetTemplateChild(MoreBtnName) as Button;
            EmojiPanel = GetTemplateChild(EmojiPanelName) as EmojiBox;
            MorePanel = GetTemplateChild(MorePanelName) as GridView;
            InnerTb = GetTemplateChild(InnerTbName) as TextBox;
            if (RecordBtn != null)
            {
                RecordBtn.Click += RecordBtn_Click;
            }
            if (EmojiBtn != null)
            {
                EmojiBtn.Click += EmojiBtn_Click;
            }
            if (MoreBtn != null)
            {
                MoreBtn.Click += MoreBtn_Click;
            }
            if (EmojiPanel != null)
            {
                EmojiPanel.SelectionChanged += EmojiBox_SelectionChanged;
            }
            if (InnerTb!= null)
            {
                InnerTb.KeyDown += InnerTb_KeyDown;
            }
            if (MorePanel != null)
            {
                MorePanel.ItemClick += MorePanel_ItemClick;
            }
        }

        private void MorePanel_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as MessageMoreItem;
            if (item == null)
            {
                return;
            }
            switch (item.Label)
            {
                case "图片":
                    _ = ImageUpload();
                    break;
                case "视频":
                    _ = VideoUpload();
                    break;
                case "文件":
                    _ = FileUpload();
                    break;
                default:
                    break;
            }
        }

        private static void OnModeChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = (d as MessageInput);
            switch (e.OldValue)
            {
                case MessageInputMode.VOICE:
                    _ = instance.StopRecordAsync();
                    break;
                case MessageInputMode.EMOJI:
                    instance.EmojiVisibility = Visibility.Collapsed;
                    break;
                case MessageInputMode.MORE:
                    instance.MoreVisibility = Visibility.Collapsed;
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
                    instance.EmojiVisibility = Visibility.Visible;
                    break;
                case MessageInputMode.MORE:
                    instance.MoreVisibility = Visibility.Visible;
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
            InnerTb.SelectedText = args.Emoji.Type > 0 ? args.Emoji.Content : $"[{args.Emoji.Name}]";
        }

        private void InnerTb_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Confirm?.Invoke(this, new MessageInputArgs(InnerTb.Text));
                InnerTb.Text = "";
            }
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

    public class MessageMoreItem
    {
        public string Label { get; set; }
        public string Icon { get; set; }

        public MessageMoreItem()
        {

        }

        public MessageMoreItem(string label, string icon)
        {
            Label = label;
            Icon = icon;
        }
    }
}
