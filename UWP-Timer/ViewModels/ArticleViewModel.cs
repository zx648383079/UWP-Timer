using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;
using UWP_Timer.Repositories;

namespace UWP_Timer.ViewModels
{
    public class ArticleViewModel: BindableBase
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
                    var data = await App.Repository.Article.GetPageAsync(search);
                    if (data == null)
                    {
                        return Tuple.Create<IList<Article>, bool>(null, false);
                    }
                    return Tuple.Create(data.Data, data.Paging.More);
                });
            });
            SortItems.Add(new TabItem("new", Constants.GetString("article_tab_new")));
            SortItems.Add(new TabItem("hot", Constants.GetString("article_tab_hot")));
            SortItems.Add(new TabItem("recommend", Constants.GetString("article_tab_best")));
            _ = LoadCategories();
        }

        private SearchForm search;

        private IncrementalLoadingCollection<Article> items;

        public IncrementalLoadingCollection<Article> Items
        {
            get { return items; }
            set { Set(ref items, value); }
        }

        private ObservableCollection<TabItem> sortItems = new ObservableCollection<TabItem>();

        public ObservableCollection<TabItem> SortItems
        {
            get => sortItems;
            set => Set(ref sortItems, value);
        }


        private ObservableCollection<TabItem> categories = new ObservableCollection<TabItem>();

        public ObservableCollection<TabItem> Categories
        {
            get => categories;
            set => Set(ref categories, value);
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
            var data = await App.Repository.Article.GetCategoriesAsync();
            if (data == null)
            {
                return;
            }
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Categories.Clear();
                Categories.Add(new TabItem(0, Constants.GetString("article_tab_all")));
                foreach (var item in data.Data)
                {
                    Categories.Add(new TabItem(item.Id, item.Name));
                }
            });
        }
    }
}
