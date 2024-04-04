using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    public class ArticleViewModel: ObservableObject
    {
        public ArticleViewModel()
        {
            search = new SearchForm()
            {
                Page = 0
            };
            Items = new IncrementalLoadingCollection<Article>(count =>
            {
                return Task.Run(async () =>
                {
                    search.Page++;
                    var data = await _api.GetPageAsync(search);
                    if (data == null)
                    {
                        return Tuple.Create<IList<Article>, bool>(null, false);
                    }
                    return Tuple.Create(data.Data, data.Paging.More);
                });
            });
            SortItems.Add(new TabItem("new", App.GetString("article_tab_new")));
            SortItems.Add(new TabItem("hot", App.GetString("article_tab_hot")));
            SortItems.Add(new TabItem("recommend", App.GetString("article_tab_best")));
            _ = LoadCategories();
        }

        private RestArticleRepository _api = App.GetService<RestArticleRepository>();
        private INotificationService _notify = App.GetService<INotificationService>();

        private SearchForm search;

        private IncrementalLoadingCollection<Article> items;

        public IncrementalLoadingCollection<Article> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        private ObservableCollection<TabItem> sortItems = [];

        public ObservableCollection<TabItem> SortItems
        {
            get => sortItems;
            set => SetProperty(ref sortItems, value);
        }


        private ObservableCollection<TabItem> categories = [];

        public ObservableCollection<TabItem> Categories
        {
            get => categories;
            set => SetProperty(ref categories, value);
        }

        public void Load()
        {
            Refresh();
        }

        public void Search(SearchForm form)
        {
            search = form;
            Load();
        }

        public void ChangeSort(string sort)
        {
            search.Sort = sort;
            search.Keywords = string.Empty;
            Load();
        }

        public void ChangeCategory(int id)
        {
            if (search.Category == id  && search.Page < 2)
            {
                return;
            }
            search.Category = id;
            search.Keywords = string.Empty;
            Load();
        }

        public void Refresh()
        {
            if (Items.IsBusy)
            {
                return;
            }
            search.Page = 0;
            items.Clear();
            _ = Items.LoadMoreItemsAsync(search.PerPage);
        }

        public async Task LoadCategories()
        {
            var data = await _api.GetCategoriesAsync();
            if (data == null)
            {
                return;
            }
            SynchronizationContext.Current!.Post(o =>
            {
                Categories.Clear();
                Categories.Add(new TabItem(0, App.GetString("article_tab_all")));
                foreach (var item in data.Data)
                {
                    Categories.Add(new TabItem(item.Id, item.Name));
                }
            }, null);
        }
    }
}
