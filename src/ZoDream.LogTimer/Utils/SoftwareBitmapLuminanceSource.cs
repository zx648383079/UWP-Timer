using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using ZXing;

namespace ZoDream.LogTimer.Utils
{
    internal class SoftwareBitmapLuminanceSource : BaseLuminanceSource
    {
        public SoftwareBitmapLuminanceSource(int width, int height) : base(width, height)
        {
        }

        public SoftwareBitmapLuminanceSource(SoftwareBitmap softwareBitmap)
           : base(softwareBitmap.PixelWidth, softwareBitmap.PixelHeight)
        {
            if (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Gray8)
            {
                using var convertedSoftwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Gray8);
                convertedSoftwareBitmap.CopyToBuffer(luminances.AsBuffer());
            }
            else
            {
                softwareBitmap.CopyToBuffer(luminances.AsBuffer());
            }
        }

        protected override LuminanceSource CreateLuminanceSource(byte[] newLuminances, int width, int height)
        {
            return new SoftwareBitmapLuminanceSource(width, height) { luminances = newLuminances };
        }
    }
}
