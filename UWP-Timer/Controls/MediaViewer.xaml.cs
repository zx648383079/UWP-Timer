using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Timer.Converters;
using UWP_Timer.Models;
using UWP_Timer.Utils;
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
    public sealed partial class MediaViewer : UserControl
    {
        public MediaViewer()
        {
            this.InitializeComponent();
        }

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

        private void RefreshView()
        {
            MediaBox.Children.Clear();
            if (Items == null)
            {
                return;
            }
            var items = Items.ToList();
            if (items.Count < 1) {
                return;
            }
            var first = items[0];
            var ext = first.File.Substring(first.File.LastIndexOf('.'));
            if (isVoice(ext))
            {
                foreach (var item in items)
                {
                    MediaBox.Children.Add(new AudioPlayer()
                    {
                        Source = item.File,
                        Height = 80
                    });
                }
            } else if (isVideo(ext))
            {
                foreach (var item in items)
                {
                    MediaBox.Children.Add(new VideoPlayer()
                    {
                        Source = item.File,
                    });
                }
            } else if (items.Count < 2)
            {
                MediaBox.Children.Add(new Image()
                {
                    Source = ConverterHelper.ToImg(first.File)
                });
            } else
            {
                MediaBox.Children.Add(new ImageViewer()
                {
                    Items = Items
                });
            }
        }

        private bool isVoice(string ext)
        {
            return Arr.Contain(ext, new string[] { ".mp3", ".wav", ".mid", ".flac", ".ape" });
        }

        private bool isVideo(string ext)
        {
            return Arr.Contain(ext, new string[] { ".flv", ".swf", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg",
        ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm"});
        }


    }
}
