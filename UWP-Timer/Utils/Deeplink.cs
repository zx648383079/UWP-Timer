using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace UWP_Timer.Utils
{
    public static class Deeplink
    {
        const string SCHAME = "zodream";

        public static void OpenLink(Frame frame, string link)
        {
            if (string.IsNullOrWhiteSpace(link) || link.StartsWith('#') || link.StartsWith("javascript:"))
            {
                return;
            }
            if (!Uri.TryCreate(link, UriKind.Absolute, out Uri uri))
            {
                return;
            }
            if (uri.Scheme == "http" || uri.Scheme == "https")
            {
                // 链接
                return;
            }
            if (uri.Scheme != SCHAME)
            {
                return;
            }
            if (uri.Host == "blog" || uri.Host == "article")
            {
                frame.Navigate(typeof(Views.Article.IndexPage));
                return;
            }
            if (uri.Host == "bulletin")
            {
                frame.Navigate(typeof(Views.Message.IndexPage));
                return;
            }
            if (uri.Host == "task")
            {
                frame.Navigate(typeof(Views.HomePage));
                return;
            }
        }
    }
}
