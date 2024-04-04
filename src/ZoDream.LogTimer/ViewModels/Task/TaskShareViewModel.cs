using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    public class TaskShareViewModel: ObservableObject
    {
        public TaskShareViewModel()
        {
            initCollection();
            initMyCollection();
        }

        #region Share
        private void initCollection()
        {
            Items = new IncrementalLoadingCollection<TaskShare>(count =>
            {
                return Task.Run(async () =>
                {
                    Queries.Page++;
                    var data = await _api.GetShareListAsync(Queries);
                    if (data == null)
                    {
                        return Tuple.Create<IList<TaskShare>, bool>(null, false);
                    }
                    return Tuple.Create(data.Data, data.Paging.More);
                });
            });
        }

        private RestTaskRepository _api = App.GetService<RestTaskRepository>();
        private INotificationService _notify = App.GetService<INotificationService>();
        private bool booted = false;

        public TaskQueries Queries = new TaskQueries();

        private IncrementalLoadingCollection<TaskShare> items;

        public IncrementalLoadingCollection<TaskShare> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        public void Refresh()
        {
            Queries.Page = 0;
            Items.Clear();
            _ = Items.LoadMoreItemsAsync(Queries.PerPage);
        }

        public void Load()
        {
            if (booted)
            {
                return;
            }
            booted = true;
            Refresh();
        }
        #endregion

        #region MyShare
        private void initMyCollection()
        {
            MyItems = new IncrementalLoadingCollection<TaskShare>(count =>
            {
                return Task.Run(async () =>
                {
                    MyQueries.Page++;
                    var data = await _api.GetShareMyAsync(MyQueries);
                    if (data == null)
                    {
                        return Tuple.Create<IList<TaskShare>, bool>(null, false);
                    }
                    return Tuple.Create(data.Data, data.Paging.More);
                });
            });
        }

        private bool bootedMy = false;
        public TaskQueries MyQueries = new();

        private IncrementalLoadingCollection<TaskShare> myItems;

        public IncrementalLoadingCollection<TaskShare> MyItems
        {
            get { return myItems; }
            set { SetProperty(ref myItems, value); }
        }

        public void RefreshMy()
        {
            MyQueries.Page = 0;
            MyItems.Clear();
            _ = MyItems.LoadMoreItemsAsync(MyQueries.PerPage);
        }

        public void LoadMy()
        {
            if (bootedMy)
            {
                return;
            }
            bootedMy = true;
            RefreshMy();
        }
        #endregion

    }
}
