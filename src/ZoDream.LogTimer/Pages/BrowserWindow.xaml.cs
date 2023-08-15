using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using ZoDream.LogTimer.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BrowserWindow : Window
    {
        public BrowserWindow()
        {
            this.InitializeComponent();
            Browser.CoreWebView2Initialized += Browser_CoreWebView2Initialized;
        }

        private void Browser_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            Browser.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
            Browser.CoreWebView2.DocumentTitleChanged += CoreWebView2_DocumentTitleChanged;
        }

        private void CoreWebView2_DocumentTitleChanged(Microsoft.Web.WebView2.Core.CoreWebView2 sender, object args)
        {
            Title = Browser.CoreWebView2.DocumentTitle;
        }

        private void CoreWebView2_NewWindowRequested(Microsoft.Web.WebView2.Core.CoreWebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs args)
        {
            args.Handled = true;
            NavigateUrl(args.Uri);
        }

        private void UrlTb_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                NavigateUrl(UrlTb.Text);
            }
        }

        private void Browser_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            UrlTb.Text = args.Uri.ToString();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Browser.CanGoBack)
            {
                Browser.GoBack();
            }
        }

        private void ForwardBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!Browser.CanGoForward)
            {
                Browser.GoForward();
            }
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            Browser.Reload();
        }

        public void NavigateUrl(string url)
        {
            url = UriRender.Render(url, 0);
            try
            {
                if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
                {
                    UrlTb.Text = url;
                    Browser.Source = uri;
                }
            }
            catch { }
        }

        public void NavigateUrl(Uri url)
        {
            UrlTb.Text = url.ToString();
            Browser.Source = url;
        }

        private void Window_Closed(object sender, WindowEventArgs args)
        {
            Browser.Close();
        }
    }
}
