using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media;

namespace ZoDream.LogTimer.Utils
{
    public class CameraFrameEventArgs: EventArgs
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        private VideoFrame _videoFrame;
        private VideoFrame _videoFrameCopy;

        /// <summary>
        /// Gets the video frame.
        /// </summary>
        public VideoFrame VideoFrame
        {
            get
            {
                _semaphore.Wait();

                // The VideoFrame could be disposed at any time so we need to create a copy we can use.
                if (_videoFrameCopy == null &&
                    _videoFrame != null &&
                    _videoFrame.SoftwareBitmap != null)
                {
                    try
                    {
                        _videoFrameCopy = VideoFrame.CreateWithSoftwareBitmap(SoftwareBitmap.Copy(_videoFrame.SoftwareBitmap));
                    }
                    catch (Exception)
                    {
                    }
                }

                _semaphore.Release();
                return _videoFrameCopy ?? _videoFrame;
            }

            internal set
            {
                _videoFrame = value;
            }
        }
    }
}
