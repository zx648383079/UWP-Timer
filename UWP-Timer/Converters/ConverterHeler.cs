using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace UWP_Timer.Converters
{
    public static class ConverterHeler
    {
        /// <summary>
        /// Returns the reverse of the provided value.
        /// </summary>
        public static bool Not(bool value) => !value;

        /// <summary>
        /// Returns true if the specified value is not null; otherwise, returns false.
        /// </summary>
        public static bool IsNotNull(object value) => value != null;

        /// <summary>
        /// Returns Visibility.Collapsed if the specified value is true; otherwise, returns Visibility.Visible.
        /// </summary>
        public static Visibility CollapsedIf(bool value) =>
            value ? Visibility.Collapsed : Visibility.Visible;

        public static Visibility VisibleIf(bool value) =>
            CollapsedIf(!value);

        /// <summary>
        /// Returns Visibility.Collapsed if the specified value is null; otherwise, returns Visibility.Visible.
        /// </summary>
        public static Visibility CollapsedIfNull(object value) =>
            value == null ? Visibility.Collapsed : Visibility.Visible;

        /// <summary>
        /// Returns Visibility.Collapsed if the specified string is null or empty; otherwise, returns Visibility.Visible.
        /// </summary>
        public static Visibility CollapsedIfNullOrEmpty(string value) =>
            string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;

        public static Visibility VisibleNotEmpty(int value) =>
            CollapsedIf(value <= 0);

        public static string FormatHour(int value)
        {
            if (value <= 0)
            {
                return "00:00";
            }
            var m = value / 60;
            if (m >= 60)
            {
                return (m / 60).ToString("00") + ":" 
                    + (m % 60).ToString("00") + ":" + (value % 60).ToString("00");
            }
            return m.ToString("00") + ":" + (value % 60).ToString("00");
        }

        public static string TwoPad(int value)
        {
            return value.ToString("00");
        }

        public static string FormatDay(int value)
        {
            if (value <= 0)
            {
                return string.Empty;
            }
            return value.ToString("00");
        }

        public static BitmapImage ToImg(string value)
        {
            var imageUrl = value;
            if (string.IsNullOrEmpty(imageUrl))
            {
                imageUrl = "Assets/Square44x44Logo.scale-200.png";
            }
            if (!imageUrl.StartsWith("http") && !imageUrl.StartsWith("ms-appx:"))
            {
                imageUrl = string.Concat("ms-appx:///", imageUrl);
            }
            return new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
        }

        public static string Icon(string name)
        {
            return "\xE082";
            switch (name)
            {
                case "qq":
                case "fa-qq":
                    return "\ue69e";
                case "weixin":
                case "wechat":
                case "fa-weixin":
                    return "\ue600";
                case "alipay":
                case "fa-alipay":
                    return "\ue602";
                case "weibo":
                case "fa-weibo":
                    return "\ue6b4";
                case "paypal":
                case "fa-paypal":
                    return "\ue905";
                case "github":
                case "fa-github":
                    return "\ue691";
                case "google":
                case "fa-google":
                    return "\ue8f1";
                default:
                    break;
            }
            return "";
        }
    }
}
