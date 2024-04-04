using Microsoft.UI.Xaml.Controls;
using System;
using ZoDream.LogTimer.Pages;

namespace ZoDream.LogTimer.Services
{
    public class Deeplink(NavigationService navigation)
    {
        const string SCHAME = "zodream";
        public bool IsSchame(string uri)
        {
            return uri.StartsWith(SCHAME);
        }

        public bool IsSchame(Uri uri)
        {
            return uri.Scheme == SCHAME;
        }

        public void OpenLink(string link)
        {
            if (string.IsNullOrWhiteSpace(link) || link.StartsWith('#') || link.StartsWith("javascript:"))
            {
                return;
            }
            if (!Uri.TryCreate(link, UriKind.Absolute, out Uri? uri))
            {
                return;
            }
            OpenLink(uri);
        }

        public void OpenLink(Uri uri)
        {
            if (uri.Scheme == "http" || uri.Scheme == "https")
            {
                // 链接
                navigation.NavigateUrl(uri);
                return;
            }
            if (!IsSchame(uri))
            {
                return;
            }
            if (uri.Host == "blog" || uri.Host == "article")
            {
                navigation.Navigate(typeof(Pages.Article.ListPage));
                return;
            }
            if (uri.Host == "bulletin")
            {
                navigation.Navigate(typeof(Pages.Message.IndexPage));
                return;
            }
            if (uri.Host == "task")
            {
                navigation.Navigate(typeof(Pages.Plan.TodayPage));
                return;
            }
            if (uri.Host == "micro")
            {
                navigation.Navigate(typeof(Pages.Micro.IndexPage));
                return;
            }
        }
    }
}
