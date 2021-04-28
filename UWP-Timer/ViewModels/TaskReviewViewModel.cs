using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;

namespace UWP_Timer.ViewModels
{
    public class TaskReviewViewModel: BindableBase
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
                    if (!App.IsLogin())
                    {
                        return Tuple.Create<IList<TaskLog>, bool>(null, false);
                    }
                    search.Page++;
                    var data = await App.Repository.Task.GetTaskReviewAsync(search);
                    if (data == null)
                    {
                        return Tuple.Create<IList<TaskLog>, bool>(null, false);
                    }
                    return Tuple.Create(data.Data, data.Paging == null ? false : data.Paging.More);
                });
            });
        }

        private SearchForm search;

        private IncrementalLoadingCollection<TaskLog> items;

        public IncrementalLoadingCollection<TaskLog> Items
        {
            get { return items; }
            set { Set(ref items, value); }
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
