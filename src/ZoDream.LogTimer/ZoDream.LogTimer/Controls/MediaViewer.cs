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
using ZoDream.LogTimer.Converters;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    [TemplatePart(Name = MainPanelName, Type = typeof(Panel))]
    public sealed class MediaViewer : Control
    {
        const string MainPanelName = "PART_MediaPanel";
        public MediaViewer()
        {
            this.DefaultStyleKey = typeof(MediaViewer);
        }

        private Panel MainPanel;

        public IEnumerable<MicroAttachment> Items
        {
            get { return (IEnumerable<MicroAttachment>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable<MicroAttachment>), typeof(MediaViewer), new PropertyMetadata(null, new PropertyChangedCallback(OnItemsChange)));

        private static void OnItemsChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MediaViewer).RefreshView();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            MainPanel = GetTemplateChild(MainPanelName) as Panel;
        }

        private void RefreshView()
        {
            MainPanel.Children.Clear();
            if (Items == null)
            {
                return;
            }
            var items = Items.ToList();
            if (items.Count < 1)
            {
                return;
            }
            var first = items[0];
            var ext = first.File.Substring(first.File.LastIndexOf('.'));
            if (IsVoice(ext))
            {
                foreach (var item in items)
                {
                    MainPanel.Children.Add(new AudioPlayer()
                    {
                        Source = item.File,
                        Height = 80
                    });
                }
            }
            else if (IsVideo(ext))
            {
                foreach (var item in items)
                {
                    MainPanel.Children.Add(new VideoPlayer()
                    {
                        Source = item.File,
                    });
                }
            }
            else if (items.Count < 2)
            {
                MainPanel.Children.Add(new Image()
                {
                    Source = ConverterHelper.ToImg(first.File)
                });
            }
            else
            {
                MainPanel.Children.Add(new ImageViewer()
                {
                    Items = Items
                });
            }
        }

        private bool IsVoice(string ext)
        {
            return Arr.Contain(ext, new string[] { ".mp3", ".wav", ".mid", ".flac", ".ape" });
        }

        private bool IsVideo(string ext)
        {
            return Arr.Contain(ext, new string[] { ".flv", ".swf", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg",
        ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm"});
        }
    }
}
