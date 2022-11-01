using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Utils;
using ZoDream.Shared.Http;
using ZoDream.Shared.Loggers;

namespace ZoDream.LogTimer.ViewModels
{
    internal class MainViewModel: BindableBase
    {

        public bool IsBooted { get; set; } = false;

        public ILogger Logger { get; private set; } = new EventLogger();

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
        public MainWindow AppWindow { get; internal set; }

        public XamlRoot XamlRoot => AppWindow?.Content.XamlRoot;

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

        public async Task ShowMessageAsync(string message, string title = "提示")
        {
            if (AppWindow == null)
            {
                return;
            }
            var dialog = new ContentDialog()
            {
                Title = title,
                Content = message,
                CloseButtonText = "Ok",
                XamlRoot = XamlRoot,
            };
            await dialog.ShowAsync();
        }

        public void FullScreenAsync(bool isFull)
        {
            
        }

        public void OpenUrlAsync(string url)
        {
            if (AppWindow == null)
            {
                return;
            }
            Deeplink.OpenLink(AppWindow.AppFrame, url);
        }
    }
}
