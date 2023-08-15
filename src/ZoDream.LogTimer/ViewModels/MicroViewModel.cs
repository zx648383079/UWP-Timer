using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories.Models;

namespace ZoDream.LogTimer.ViewModels
{
    public class MicroViewModel : BindableBase
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
                    var data = await App.Repository.Micro.GetPageAsync(search);
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

        private MicroQueries search;


        private ObservableCollection<TabItem> statusItems = new ObservableCollection<TabItem>();

        public ObservableCollection<TabItem> StatusItems
        {
            get => statusItems;
            set => Set(ref statusItems, value);
        }


        private IncrementalLoadingCollection<MicroItem> items;

        public IncrementalLoadingCollection<MicroItem> Items
        {
            get { return items; }
            set { Set(ref items, value); }
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
