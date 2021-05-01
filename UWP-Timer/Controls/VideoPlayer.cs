using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Timer.Utils;
using Windows.Media.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace UWP_Timer.Controls
{
    [TemplatePart(Name = "Cover", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "Player", Type = typeof(Panel))]
    public sealed class VideoPlayer : Control
    {
        public VideoPlayer()
        {
            this.DefaultStyleKey = typeof(VideoPlayer);
            Loaded += VideoPlayer_Loaded;
        }

        public bool IsPlaying { get; private set; } = false;

        private void VideoPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            var cover = GetTemplateChild("Cover") as FrameworkElement;
            if (cover != null)
            {
                cover.Tapped += Cover_Tapped;
            }
        }

        private void Cover_Tapped(object sender, TappedRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Playing", false);
            IsPlaying = true;
            RefreshPlayerView();
        }

        private void RefreshPlayerView()
        {
            if (!IsPlaying)
            {
                return;
            }
            var player = GetTemplateChild("Player") as Panel;
            if (player == null || Source == null)
            {
                return;
            }
            Uri src;
            if (Source.GetType() == typeof(string))
            {
                var source = (string)Source;
                if (string.IsNullOrWhiteSpace(source))
                {
                    return;
                }
                if (source.StartsWith("//"))
                {
                    source = "https:" + source;
                }
                src = new Uri(source);
            } else
            {
                src = (Uri)Source;
            }
            if (src == null)
            {
                return;
            }
            player.Children.Clear();
            if (playerType(src))
            {
                var webview = new WebView
                {
                    Source = src
                };
                player.Children.Add(webview);
                return;
            }
            var video = new MediaPlayerElement
            {
                Source = MediaSource.CreateFromUri(src),
                AutoPlay = true,
                AreTransportControlsEnabled = true
            };
            player.Children.Add(video);
        }

        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(object), typeof(VideoPlayer), new PropertyMetadata(null, new PropertyChangedCallback(OnSourceChange)));

        private static void OnSourceChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VideoPlayer).RefreshPlayerView();
        }

        private bool playerType(Uri src) {
            return Arr.Contain(src.Host, new string[] { "player.youku.com", "player.bilibili.com", "v.qq.com", "open.iqiyi.com" });
        }
}
}
