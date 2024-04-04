using CommunityToolkit.Mvvm.Input;
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
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.WebUI;
using ZoDream.LogTimer.Services;
using ZoDream.LogTimer.ViewModels;
using static Vanara.PInvoke.User32.RAWINPUT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Article
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        public DetailPage()
        {
            this.InitializeComponent();
            ViewModel.RegisterRender(new RelayCommand<string>(RenderHtml));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var id = (int)e.Parameter;
            _ = ViewModel.LoadArticleAsync(id);
        }

        
        private async void RenderHtml(string? html)
        {
            await detailWebView.EnsureCoreWebView2Async();
            detailWebView.NavigateToString(html);
            var data = ViewModel.Article;
            if (!string.IsNullOrEmpty(data.VideoUrl))
            {
                Video.Visibility = Visibility.Visible;
                Video.Source = data.VideoUrl;
            }
            else
            {
                Video.Visibility = Visibility.Collapsed;
            }
        }

        private void detailWebView_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if (args.Uri == null || args.Uri.StartsWith("data:"))
            {
                return;
            }
            args.Cancel = true;
            ViewModel.Navigate(args.Uri);
        }
    }
}
