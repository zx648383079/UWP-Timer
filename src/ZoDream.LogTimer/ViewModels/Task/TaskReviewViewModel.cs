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
    public class TaskReviewViewModel: ObservableObject
    {
        public TaskReviewViewModel()
        {
            search = new SearchForm()
            {
                Page = 0,
                Type = "day",
            };
            Items = new IncrementalLoadingCollection<TaskLog>(count =>
            {
                return Task.Run(async () =>
                {
                    if (!_auth.Authenticated)
                    {
                        return Tuple.Create<IList<TaskLog>, bool>(null, false);
                    }
                    search.Page++;
                    var data = await _api.GetTaskReviewAsync(search);
                    if (data == null)
                    {
                        return Tuple.Create<IList<TaskLog>, bool>(null, false);
                    }
                    return Tuple.Create(data.Data, data.Paging == null ? false : data.Paging.More);
                });
            });
        }
        private IAuthService _auth = App.GetService<IAuthService>();
        private RestTaskRepository _api = App.GetService<RestTaskRepository>();
        private INotificationService _notify = App.GetService<INotificationService>();

        private SearchForm search;

        private IncrementalLoadingCollection<TaskLog> items;

        public IncrementalLoadingCollection<TaskLog> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        public void Load()
        {
            Refresh();
        }

        public void ChangeDate(string date)
        {
            search.Date = date;
            Load();
        }

        public void Refresh()
        {
            search.Page = 0;
            items.Clear();
            _ = Items.LoadMoreItemsAsync(search.PerPage);
        }
    }
}
