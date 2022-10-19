using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;

namespace ZoDream.LogTimer.Utils
{
    public class AudioRecorder
    {
        private MediaCapture mediaCapture;
        public InMemoryRandomAccessStream Stream { get; private set; }
        public bool IsRecording { get; private set; }

        public async Task StartAsync()
        {
            if (IsRecording)
            {
                return;
            }
            Stream = new InMemoryRandomAccessStream();
            mediaCapture = new MediaCapture();
            var settings = new MediaCaptureInitializationSettings()
            {
                StreamingCaptureMode = StreamingCaptureMode.Audio,
                AudioProcessing = Windows.Media.AudioProcessing.Raw
            };
            await mediaCapture.InitializeAsync(settings);
            await mediaCapture.StartRecordToStreamAsync(MediaEncodingProfile.CreateMp3(AudioEncodingQuality.Auto), Stream);
            IsRecording = true;
        }

        public async Task StopAsync()
        {
            if (!IsRecording)
            {
                return;
            }
            await mediaCapture.StopRecordAsync();
            IsRecording = false;
        }

        public void Dispose()
        {
            mediaCapture?.Dispose();
            Stream?.Dispose();
        }
    }
}
