using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Utils;
using UWP_Timer.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP_Timer.Views.Article
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
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
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            await dispatcherQueue.EnqueueAsync(async () =>
            {
                App.ViewModel.IsLoading = false;
                if (data == null)
                {
                    Frame.GoBack();
                    return;
                }
                ViewModel.Article = data;
                detailWebView.NavigateToString(await RenderHtmlAsync(data.Content));
                if (!string.IsNullOrEmpty(data.VideoUrl))
                {
                    Video.Visibility = Visibility.Visible;
                    Video.Source = data.VideoUrl;
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

        private void detailWebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (args.Uri != null)
            {
                args.Cancel = true;
            }
        }
    }
}
