using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace UWP_Timer.Controls
{
    [TemplatePart(Name = "ActionButton", Type = typeof(IconElement))]
    [TemplatePart(Name = "ProgressSlider", Type = typeof(Slider))]
    [TemplatePart(Name = "ProgressLabel", Type = typeof(TextBlock))]
    [TemplatePart(Name = "VolumnButton", Type = typeof(IconElement))]
    [TemplatePart(Name = "VolumnSlider", Type = typeof(Slider))]
    public sealed class AudioPlayer : Control
    {
        public AudioPlayer()
        {
            this.DefaultStyleKey = typeof(AudioPlayer);
            Loaded += AudioPlayer_Loaded;
            Unloaded += AudioPlayer_Unloaded;
        }

        private void AudioPlayer_Unloaded(object sender, RoutedEventArgs e)
        {
            if (playerInstance != null)
            {
                playerInstance.Dispose();
            }
        }

        private void AudioPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            var actionBtn = GetTemplateChild("ActionButton") as IconElement;
            var volumnBtn = GetTemplateChild("VolumnButton") as IconElement;
            var progressSlider = GetTemplateChild("ProgressSlider") as Slider;
            var volumnSlider = GetTemplateChild("VolumnSlider") as Slider;
            if (actionBtn != null)
            {
                actionBtn.Tapped += ActionBtn_Tapped;
            }
            if (volumnBtn != null)
            {
                volumnBtn.Tapped += VolumnBtn_Tapped;
            }
            if (progressSlider != null)
            {
                progressSlider.ValueChanged += ProgressSlider_ValueChanged;
            }
            if (volumnSlider != null)
            {
                volumnSlider.ValueChanged += VolumnSlider_ValueChanged;
            }
        }

        private void VolumnSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (!IsPlaying)
            {
                return;
            }
            Player().Volume = (sender as Slider).Value / 100;
        }

        private void ProgressSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (!IsPlaying)
            {
                return;
            }
            Player().PlaybackSession.Position = Player().PlaybackSession.NaturalDuration * ((sender as Slider).Value / 100);
        }

        private void VolumnBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!IsPlaying)
            {
                return;
            }
            var player = Player();
            if (player.Volume <= 0)
            {
                player.Volume = lastVolumn / 100;
            }
            else {
                lastVolumn = player.Volume * 100;
                player.Volume = 0;
            }
        }

        private void ActionBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var btn = sender as FontIcon;
            if (IsPlaying)
            {
                Player().Pause();
                IsPlaying = false;
                btn.Glyph = "\xF5B0;";
                return;
            }
            if (playerInstance == null)
            {
                Player().Source = MediaSource.CreateFromUri(Source is Uri ? Source as Uri : new Uri(Source.ToString()));
            }
            Player().Play();
            IsPlaying = true;
            btn.Glyph = "\xE15B;";
        }

        private MediaPlayer playerInstance;
        public bool IsPlaying { get; private set; } = false;
        private double lastVolumn = 100;
        private MediaPlayer Player()
        {
            if (playerInstance != null)
            {
                return playerInstance;
            }
            playerInstance = new MediaPlayer();
            playerInstance.VolumeChanged += PlayerInstance_VolumeChanged;
            playerInstance.MediaEnded += PlayerInstance_MediaEnded;
            playerInstance.PlaybackSession.PositionChanged += PlaybackSession_PositionChanged;
            return playerInstance;
        }

        private void PlaybackSession_PositionChanged(MediaPlaybackSession sender, object args)
        {
            var progressSlider = GetTemplateChild("ProgressSlider") as Slider;
            if (progressSlider == null)
            {
                return;
            }
            var scale = sender.Position / sender.NaturalDuration * 100;
            progressSlider.Value = scale;
            var label = GetTemplateChild("ProgressLabel") as TextBlock;
            if (label == null)
            {
                return;
            }
            label.Text = sender.Position.ToString("mm:ss") + "/" + sender.NaturalDuration.ToString("mm:ss");
        }

        private void PlayerInstance_MediaEnded(MediaPlayer sender, object args)
        {
            IsPlaying = false;
        }

        private void PlayerInstance_VolumeChanged(MediaPlayer sender, object args)
        {
            var volumnSlider = GetTemplateChild("VolumnSlider") as Slider;
            if (volumnSlider == null)
            {
                return;
            }
            volumnSlider.Value = sender.Volume * 100;
            var volumnBtn = GetTemplateChild("VolumnButton") as FontIcon;
            if (volumnBtn == null)
            {
                return;
            }
            if (sender.Volume <= 0)
            {
                volumnBtn.Glyph = "\xE992;";
            }
            else if (sender.Volume < 40)
            {
                volumnBtn.Glyph = "\xE993;";
            }
            else if (sender.Volume < 60)
            {
                volumnBtn.Glyph = "\xE994;";
            } else
            {
                volumnBtn.Glyph = "\xE995;";
            }
        }

        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(object), typeof(AudioPlayer), new PropertyMetadata(null, new PropertyChangedCallback(OnSourceChange)));

        private static void OnSourceChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AudioPlayer).RefreshPlayerView();
        }

        private void RefreshPlayerView()
        {
            if (IsPlaying)
            {

            }
        }
    }

}
