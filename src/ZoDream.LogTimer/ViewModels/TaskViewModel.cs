using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;

namespace ZoDream.LogTimer.ViewModels
{
    public class TaskViewModel: BindableBase
    {
        public TaskViewModel()
        {
            search = new SearchForm()
            {
                Page = 0,
                Status = 1
            };
            Items = new IncrementalLoadingCollection<TaskItem>(count =>
            {
                return Task.Run(async () =>
                {
                    search.Page++;
                    var data = await App.Repository.Task.GetTaskAsync(search);
                    if (data == null)
                    {
                        return Tuple.Create<IList<TaskItem>, bool>(null, false);
                    }
                    return Tuple.Create(data.Data, data.Paging.More);
                });
            });
            StatusItems.Add(new TabItem(1, Constants.GetString("task_tab_1")));
            StatusItems.Add(new TabItem(2, Constants.GetString("task_tab_2")));
        }

        private SearchForm search;

        private bool inEdit = false;

        public bool InEdit
        {
            get => inEdit;
            set => Set(ref inEdit, value);
        }

        private ObservableCollection<TabItem> statusItems = new ObservableCollection<TabItem>();

        public ObservableCollection<TabItem> StatusItems
        {
            get => statusItems;
            set => Set(ref statusItems, value);
        }


        private IncrementalLoadingCollection<TaskItem> items;

        public IncrementalLoadingCollection<TaskItem> Items
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
