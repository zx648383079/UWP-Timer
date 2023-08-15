using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.Capture.Frames;
using Windows.Media.Core;
using Windows.Media.Playback;
using ZoDream.LogTimer.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    [TemplatePart(Name = MediaPlayerName, Type = typeof(MediaPlayerElement))]
    [TemplatePart(Name = FrameSourceGroupBtnName, Type = typeof(Button))]
    public sealed class CameraPreview : Control
    {
        const string MediaPlayerName = "PART_MediaPlayer";
        const string FrameSourceGroupBtnName = "PART_FrameSourceGroupBtn";

        public CameraPreview()
        {
            this.DefaultStyleKey = typeof(CameraPreview);
        }

        // public event EventHandler<PreviewFailedEventArgs> PreviewFailed;

        private CameraHelper _cameraHelper;
        private MediaPlayer _mediaPlayer;
        private MediaPlayerElement _mediaPlayerElementControl;
        private Button _frameSourceGroupButton;

        private IReadOnlyList<MediaFrameSourceGroup> _frameSourceGroups;

        private bool IsFrameSourceGroupButtonAvailable => _frameSourceGroups != null && _frameSourceGroups.Count > 1;

        /// <summary>
        /// Gets Camera Helper
        /// </summary>
        public CameraHelper CameraHelper { get => _cameraHelper; private set => _cameraHelper = value; }


        /// <summary>
        /// Using a DependencyProperty as the backing store for <see cref="IsFrameSourceGroupButtonVisible "/>.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty IsFrameSourceGroupButtonVisibleProperty =
            DependencyProperty.Register("IsFrameSourceGroupButtonVisible", typeof(bool), typeof(CameraPreview), new PropertyMetadata(true, IsFrameSourceGroupButtonVisibleChanged));

        /// <summary>
        /// Gets or sets a value indicating whether Frame Source Group Button is visible or not
        /// </summary>
        public bool IsFrameSourceGroupButtonVisible
        {
            get { return (bool)GetValue(IsFrameSourceGroupButtonVisibleProperty); }
            set { SetValue(IsFrameSourceGroupButtonVisibleProperty, value); }
        }

        private static void IsFrameSourceGroupButtonVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cameraPreview = d as CameraPreview;
            if (cameraPreview._frameSourceGroupButton != null)
            {
                cameraPreview.SetFrameSourceGroupButtonVisibility();
            }
        }

        public event EventHandler<PreviewFailedEventArgs> PreviewFailed;


        /// <summary>
        /// Initialize control with a default CameraHelper instance for video preview and frame capture.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task StartAsync()
        {
            await StartAsync(new CameraHelper());
        }

        /// <summary>
        /// Initialize control with a CameraHelper instance for video preview and frame capture.
        /// </summary>
        /// <param name="cameraHelper"><see cref="CameraHelper"/></param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task StartAsync(CameraHelper cameraHelper)
        {
            if (cameraHelper == null)
            {
                cameraHelper = new CameraHelper();
            }

            _cameraHelper = cameraHelper;
            _frameSourceGroups = await CameraHelper.GetFrameSourceGroupsAsync();

            // UI controls exist and are initialized
            if (_mediaPlayerElementControl != null)
            {
                await InitializeAsync();
            }
        }

        /// <inheritdoc/>
        protected async override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_frameSourceGroupButton != null)
            {
                _frameSourceGroupButton.Click -= FrameSourceGroupButton_ClickAsync;
            }

            _mediaPlayerElementControl = (MediaPlayerElement)GetTemplateChild(MediaPlayerName);
            _frameSourceGroupButton = (Button)GetTemplateChild(FrameSourceGroupBtnName);

            if (_frameSourceGroupButton != null)
            {
                _frameSourceGroupButton.Click += FrameSourceGroupButton_ClickAsync;
                _frameSourceGroupButton.IsEnabled = false;
                _frameSourceGroupButton.Visibility = Visibility.Collapsed;
            }

            if (_cameraHelper != null)
            {
                await InitializeAsync();
            }
        }

        private async Task InitializeAsync()
        {
            var result = await _cameraHelper.InitializeAndStartCaptureAsync();
            if (result != CameraHelperResult.Success)
            {
                InvokePreviewFailed(result.ToString());
            }

            SetUIControls(result);
        }

        private async void FrameSourceGroupButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            var oldGroup = _cameraHelper.FrameSourceGroup;
            var currentIndex = _frameSourceGroups.Select((grp, index) => new { grp, index }).First(v => v.grp.Id == oldGroup.Id).index;
            var newIndex = currentIndex < (_frameSourceGroups.Count - 1) ? currentIndex + 1 : 0;
            var group = _frameSourceGroups[newIndex];
            _frameSourceGroupButton.IsEnabled = false;
            _cameraHelper.FrameSourceGroup = group;
            await InitializeAsync();
        }

        private void InvokePreviewFailed(string error)
        {
            EventHandler<PreviewFailedEventArgs> handler = PreviewFailed;
            handler?.Invoke(this, new PreviewFailedEventArgs { Error = error });
        }

        private void SetMediaPlayerSource()
        {
            try
            {
                var frameSource = _cameraHelper?.PreviewFrameSource;
                if (frameSource != null)
                {
                    if (_mediaPlayer == null)
                    {
                        _mediaPlayer = new MediaPlayer
                        {
                            AutoPlay = true,
                            RealTimePlayback = true
                        };
                    }

                    _mediaPlayer.Source = MediaSource.CreateFromMediaFrameSource(frameSource);
                    _mediaPlayerElementControl.SetMediaPlayer(_mediaPlayer);
                }
            }
            catch (Exception ex)
            {
                InvokePreviewFailed(ex.Message);
            }
        }

        private void SetUIControls(CameraHelperResult result)
        {
            var success = result == CameraHelperResult.Success;
            if (success)
            {
                SetMediaPlayerSource();
            }
            else
            {
                _mediaPlayerElementControl.SetMediaPlayer(null);
            }

            _frameSourceGroupButton.IsEnabled = IsFrameSourceGroupButtonAvailable;
            SetFrameSourceGroupButtonVisibility();
        }

        private void SetFrameSourceGroupButtonVisibility()
        {
            _frameSourceGroupButton.Visibility = IsFrameSourceGroupButtonAvailable && IsFrameSourceGroupButtonVisible
                                                                ? Visibility.Visible
                                                                : Visibility.Collapsed;
        }

        /// <summary>
        /// Stops preview.
        /// </summary>
        public void Stop()
        {
            if (_mediaPlayerElementControl != null)
            {
                _mediaPlayerElementControl.SetMediaPlayer(null);
            }

            if (_mediaPlayer != null)
            {
                _mediaPlayer.Dispose();
                _mediaPlayer = null;
            }
        }
    }

    public class PreviewFailedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets error information about failure
        /// </summary>
        public string Error { get; internal set; }
    }
}
