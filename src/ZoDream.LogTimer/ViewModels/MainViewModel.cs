using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    internal class MainViewModel: ObservableObject
    {

        public MainViewModel()
        {
            var notify = App.GetService<INotificationService>();
            notify.RegisterLoading(new RelayCommand<bool>(load => IsLoading = load));
        }

        private bool _isLoading = false;

        /// <summary>
        /// Gets or sets a value indicating whether the Customers list is currently being updated. 
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private ObservableCollection<TabItem> tipItems = [];

        public ObservableCollection<TabItem> TipItems
        {
            get { return tipItems; }
            set { SetProperty(ref tipItems, value); }
        }

        public async Task LoadTipAsync(string keywords)
        {
            var data = await App.GetService<RestArticleRepository>()!.GetSuggestionAsync(keywords);
            SynchronizationContext.Current?.Post(o =>
            {
                TipItems.Clear();
                foreach (var item in data.Data)
                {
                    TipItems.Add(new TabItem(item.Id, item.Title));
                }
            }, null);
        }

    }
}
