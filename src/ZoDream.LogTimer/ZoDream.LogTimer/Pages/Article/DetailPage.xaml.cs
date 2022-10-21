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
using ZoDream.LogTimer.Utils;
using ZoDream.LogTimer.ViewModels;

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
        }

        public ArticleDetailViewModel ViewModel = new ArticleDetailViewModel();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var id = (int)e.Parameter;
            _ = LoadArticleAsync(id);
        }

        private async Task LoadArticleAsync(int id)
        {
            App.ViewModel.IsLoading = true;
            var data = await App.Repository.Article.GetArticleAsync(id);
            DispatcherQueue.TryEnqueue(async () =>
            {
                App.ViewModel.IsLoading = false;
                if (data == null)
                {
                    Frame.GoBack();
                    return;
                }
                ViewModel.Article = data;
                await detailWebView.EnsureCoreWebView2Async();
                detailWebView.NavigateToString(await RenderHtmlAsync(data.Content));
                if (!string.IsNullOrEmpty(data.VideoUrl))
                {
                    Video.Visibility = Visibility.Visible;
                    Video.Source = data.VideoUrl;
                }
                else
                {
                    Video.Visibility = Visibility.Collapsed;
                }
            });

        }

        private async Task<string> RenderHtmlAsync(string content)
        {
            string style;
            try
            {
                var fileUri = new Uri("ms-appx:///Assets/markdown.css", UriKind.Absolute);
                var file = await StorageFile.GetFileFromApplicationUriAsync(fileUri);
                style = await FileIO.ReadTextAsync(file);
            }
            catch (Exception)
            {
                style = string.Empty;
            }
            return $"<style>{style}</style><div class=\"markdown\">{content}</div>";
        }

        private void detailWebView_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if (args.Uri == null || args.Uri.StartsWith("data:"))
            {
                return;
            }
            args.Cancel = true;
            if (Deeplink.IsSchame(args.Uri))
            {
                Deeplink.OpenLink(Frame, args.Uri);
            }
        }
    }
}
