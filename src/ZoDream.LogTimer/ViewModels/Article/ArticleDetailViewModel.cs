using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    public class ArticleDetailViewModel: ObservableObject
    {
        private AppViewModel _app = App.GetService<AppViewModel>();
        private INotificationService _notify = App.GetService<INotificationService>();
        private NavigationService _router = App.GetService<NavigationService>();
        private RestArticleRepository _api = App.GetService<RestArticleRepository>();
        private Deeplink _deep = App.GetService<Deeplink>();

        private Article article = new() {
            User = new User(),
            Term = new ArticleCategory()
        };

        public Article Article
        {
            get => article;
            set => SetProperty(ref article, value);
        }

        private ICommand? RenderCommand;

        public void RegisterRender(ICommand renderCommand)
        {
            RenderCommand = renderCommand;
        }

        public async Task LoadArticleAsync(int id)
        {
            _notify.Loading(true);
            var data = await _api.GetArticleAsync(id);
            _app.DispatcherQueue.TryEnqueue(async () => {
                _notify.Loading(false);
                if (data == null)
                {
                    _router.GoBack();
                    return;
                }
                Article = data;
                RenderCommand?.Execute(await RenderHtmlAsync(data.Content));
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

        public void Navigate(string url) 
        {
            if (_deep.IsSchame(url))
            {
                _deep.OpenLink(url);
            }
        }
    }
}
