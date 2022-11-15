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
using Windows.Media.Core;
using Windows.UI.WebUI;
using ZoDream.LogTimer.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    [TemplatePart(Name = CoverTbName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = InnerBoxName, Type = typeof(Panel))]
    public sealed class VideoPlayer : Control
    {
        const string CoverTbName = "PART_CoverTb";
        const string InnerBoxName = "PART_InnerBox";

        public VideoPlayer()
        {
            this.DefaultStyleKey = typeof(VideoPlayer);
            Unloaded += VideoPlayer_Unloaded;
        }

        private void VideoPlayer_Unloaded(object sender, RoutedEventArgs e)
        {
            if (InnerBox != null)
            {
                foreach (var item in InnerBox.Children)
                {
                    if (item is WebView2 browser)
                    {
                        browser.Close();
                    }
                }
            }
        }

        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(object), typeof(VideoPlayer), new PropertyMetadata(null, new PropertyChangedCallback(OnSourceChange)));


        private FrameworkElement CoverTb;
        private Panel InnerBox; 
        public bool Paused { get; private set; } = true;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CoverTb = GetTemplateChild(CoverTbName) as FrameworkElement;
            InnerBox = GetTemplateChild(InnerBoxName) as Panel;
            if (CoverTb != null) 
            {
                CoverTb.Tapped += CoverTb_Tapped;
            }
        }

        private void CoverTb_Tapped(object sender, TappedRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Playing", false);
            Paused = false;
            RefreshPlayerView();
        }

        private static void OnSourceChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // (d as VideoPlayer).RefreshPlayerView();
        }

        private void RefreshPlayerView()
        {
            if (Paused)
            {
                return;
            }
            var player = InnerBox;
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
            }
            else
            {
                src = (Uri)Source;
            }
            if (src == null)
            {
                return;
            }
            var isWeb = playerType(src);
            foreach (var item in player.Children)
            {
                if (item is WebView2 browser)
                {
                    if (isWeb)
                    {
                        browser.Source = src;
                        return;
                    }
                    browser.Close();
                }
                if (item is MediaPlayerElement mediaPlayer)
                {
                    if (!isWeb)
                    {
                        mediaPlayer.Source = MediaSource.CreateFromUri(src);
                        return;
                    }
                }
            }
            player.Children.Clear();
            if (isWeb)
            {
                var webview = new WebView2
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

        private bool playerType(Uri src)
        {
            return Arr.Contain(src.Host, new string[] { "player.youku.com", "player.bilibili.com", "v.qq.com", "open.iqiyi.com" });
        }


    }
}
