using Microsoft.UI.Xaml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Utils;
using ZoDream.Shared.Http;

namespace ZoDream.LogTimer.ViewModels
{
    internal class MainViewModel: BindableBase
    {
        private bool _isLoading = false;

        /// <summary>
        /// Gets or sets a value indicating whether the Customers list is currently being updated. 
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private ObservableCollection<TabItem> tipItems = new();

        public ObservableCollection<TabItem> TipItems
        {
            get { return tipItems; }
            set { Set(ref tipItems, value); }
        }

        public async Task LoadTipAsync(string keywords)
        {
            var data = await App.Repository.Article.GetSuggestionAsync(keywords);
            SynchronizationContext.Current.Post(o =>
            {
                TipItems.Clear();
                foreach (var item in data.Data)
                {
                    TipItems.Add(new TabItem(item.Id, item.Title));
                }
            }, null);
        }


        public void DefaultFailureRequest(HttpException ex)
        {
            if (ex.Code == 401)
            {
                App.Store.Auth.LogoutAsync();
            }
            if (!string.IsNullOrWhiteSpace(ex.Message))
            {
                Toast.Tip(ex.Message);
            }
        }
    }
}
