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
using Windows.Media.Core;
using Windows.Media.Playback;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    [TemplatePart(Name = ActionBtnName, Type = typeof(IconElement))]
    [TemplatePart(Name = ProgressSliderName, Type = typeof(Slider))]
    [TemplatePart(Name = ProgressTbName, Type = typeof(TextBlock))]
    [TemplatePart(Name = VolumnBtnName, Type = typeof(FontIcon))]
    [TemplatePart(Name = VolumnSliderName, Type = typeof(Slider))]
    public sealed class AudioPlayer : Control
    {
        const string ActionBtnName = "PART_ActionBtn";
        const string ProgressSliderName = "PART_ProgressSlider";
        const string ProgressTbName = "PART_ProgressTb";
        const string VolumnBtnName = "PART_VolumnBtn";
        const string VolumnSliderName = "PART_VolumnSlider";
        public AudioPlayer()
        {
            this.DefaultStyleKey = typeof(AudioPlayer);
            Unloaded += AudioPlayer_Unloaded;
        }

        private void AudioPlayer_Unloaded(object sender, RoutedEventArgs e)
        {
            PlayerInstance?.Dispose();
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

        private IconElement ActionBtn;
        private Slider ProgressSlider;
        private Slider VolumnSlider;
        private TextBlock ProgressTb;
        private FontIcon VolumnBtn;

        private MediaPlayer PlayerInstance;
        public bool Paused { get; private set; } = true;
        private double lastVolumn = 100;
        private MediaPlayer Player()
        {
            if (PlayerInstance != null)
            {
                return PlayerInstance;
            }
            PlayerInstance = new MediaPlayer();
            PlayerInstance.VolumeChanged += PlayerInstance_VolumeChanged;
            PlayerInstance.MediaEnded += PlayerInstance_MediaEnded;
            PlayerInstance.PlaybackSession.PositionChanged += PlaybackSession_PositionChanged;
            return PlayerInstance;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ActionBtn = GetTemplateChild(ActionBtnName) as IconElement;
            VolumnBtn = GetTemplateChild(VolumnBtnName) as FontIcon;
            ProgressSlider = GetTemplateChild(ProgressSliderName) as Slider;
            VolumnSlider = GetTemplateChild(VolumnSliderName) as Slider;
            if (ActionBtn != null)
            {
                ActionBtn.Tapped += ActionBtn_Tapped;
            }
            if (VolumnBtn != null)
            {
                VolumnBtn.Tapped += VolumnBtn_Tapped;
            }
            if (ProgressSlider != null)
            {
                ProgressSlider.ValueChanged += ProgressSlider_ValueChanged;
            }
            if (VolumnSlider != null)
            {
                VolumnSlider.ValueChanged += VolumnSlider_ValueChanged;
            }
        }

        private void VolumnSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Paused)
            {
                return;
            }
            Player().Volume = (sender as Slider).Value / 100;
        }

        private void ProgressSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Paused)
            {
                return;
            }
            Player().PlaybackSession.Position = Player().PlaybackSession.NaturalDuration * ((sender as Slider).Value / 100);
        }

        private void VolumnBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Paused)
            {
                return;
            }
            var player = Player();
            if (player.Volume <= 0)
            {
                player.Volume = lastVolumn / 100;
            }
            else
            {
                lastVolumn = player.Volume * 100;
                player.Volume = 0;
            }
        }

        private void ActionBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var btn = sender as FontIcon;
            if (!Paused)
            {
                Player().Pause();
                Paused = true;
                btn.Glyph = "\xF5B0;";
                return;
            }
            if (PlayerInstance == null)
            {
                Player().Source = MediaSource.CreateFromUri(Source is Uri ? Source as Uri : new Uri(Source.ToString()));
            }
            Player().Play();
            Paused = false;
            btn.Glyph = "\xE15B;";
        }

        private void PlaybackSession_PositionChanged(MediaPlaybackSession sender, object args)
        {
            var progressSlider = ProgressSlider;
            if (progressSlider == null)
            {
                return;
            }
            var scale = sender.Position / sender.NaturalDuration * 100;
            progressSlider.Value = scale;
            var label = ProgressTb;
            if (label == null)
            {
                return;
            }
            label.Text = sender.Position.ToString("mm:ss") + "/" + sender.NaturalDuration.ToString("mm:ss");
        }

        private void PlayerInstance_MediaEnded(MediaPlayer sender, object args)
        {
            Paused = true;
        }

        private void PlayerInstance_VolumeChanged(MediaPlayer sender, object args)
        {
            var volumnSlider = VolumnSlider;
            if (volumnSlider == null)
            {
                return;
            }
            volumnSlider.Value = sender.Volume * 100;
            var volumnBtn = VolumnBtn;
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
            }
            else
            {
                volumnBtn.Glyph = "\xE995;";
            }
        }

        private void RefreshPlayerView()
        {
            //if (IsPlaying)
            //{

            //}
        }
    }
}
