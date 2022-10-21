using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Utils
{
    public static class Deeplink
    {
        const string SCHAME = "zodream";

        public static bool IsSchame(string uri)
        {
            return uri.StartsWith(SCHAME);
        }

        public static bool IsSchame(Uri uri)
        {
            return uri.Scheme == SCHAME;
        }

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
            OpenLink(frame, uri);
        }

        public static void OpenLink(Frame frame, Uri uri)
        {
            if (uri.Scheme == "http" || uri.Scheme == "https")
            {
                // 链接
                return;
            }
            if (!IsSchame(uri))
            {
                return;
            }
            if (uri.Host == "blog" || uri.Host == "article")
            {
                frame.Navigate(typeof(Pages.Article.ListPage));
                return;
            }
            if (uri.Host == "bulletin")
            {
                frame.Navigate(typeof(Pages.Message.IndexPage));
                return;
            }
            if (uri.Host == "task")
            {
                frame.Navigate(typeof(Pages.Plan.TodayPage));
                return;
            }
            if (uri.Host == "micro")
            {
                frame.Navigate(typeof(Pages.Micro.IndexPage));
                return;
            }
        }
    }
}
