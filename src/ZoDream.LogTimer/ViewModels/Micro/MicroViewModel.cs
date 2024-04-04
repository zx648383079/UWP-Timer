using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    public class MicroViewModel : ObservableObject
    {
        public MicroViewModel()
        {
            search = new MicroQueries()
            {
                Page = 0,
                Status = 1
            };
            Items = new IncrementalLoadingCollection<MicroItem>(count =>
            {
                return Task.Run(async () =>
                {
                    search.Page++;
                    var data = await _api.GetPageAsync(search);
                    if (data == null)
                    {
                        return Tuple.Create<IList<MicroItem>, bool>(null, false);
                    }
                    return Tuple.Create(data.Data, data.Paging.More);
                });
            });
            StatusItems.Add(new TabItem(1, "推荐"));
            StatusItems.Add(new TabItem(2, "关注"));
            StatusItems.Add(new TabItem(3, "热门"));
            StatusItems.Add(new TabItem(4, "话题"));
        }

        private RestMicroRepository _api = App.GetService<RestMicroRepository>();
        private INotificationService _notify = App.GetService<INotificationService>();

        private MicroQueries search;


        private ObservableCollection<TabItem> statusItems = [];

        public ObservableCollection<TabItem> StatusItems
        {
            get => statusItems;
            set => SetProperty(ref statusItems, value);
        }


        private IncrementalLoadingCollection<MicroItem> items;

        public IncrementalLoadingCollection<MicroItem> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        public void Load(int status = 1)
        {
            search.Status = status;
            Refresh();
        }

        public void Refresh()
        {
            search.Page = 0;
            items.Clear();
            _ = Items.LoadMoreItemsAsync(search.PerPage);
        }
    }
}
