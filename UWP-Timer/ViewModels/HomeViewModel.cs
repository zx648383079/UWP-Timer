using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;

namespace UWP_Timer.ViewModels
{
    public class HomeViewModel: BindableBase
    {

        public HomeViewModel()
        {
            search = new SearchForm()
            {
                Page = 0
            };
            Items = new IncrementalLoadingCollection<TaskDay>(count =>
            {
                return Task.Run(async () =>
                {
                    search.Page++;
                    var data = await App.Repository.Task.GetTaskDayListAsync(search);
                    if (data == null)
                    {
                        return Tuple.Create<IList<TaskDay>, bool>(null, false);
                    }
                    return Tuple.Create(data.Data, data.Paging.More);
                });
            });
        }

        private SearchForm search;

        private IncrementalLoadingCollection<TaskDay> items;

        public IncrementalLoadingCollection<TaskDay> Items
        {
            get { return items; }
            set { Set(ref items, value); }
        }

        public void Load()
        {
            Refresh();
        }

        public void Refresh()
        {
            search.Page = 0;
            _ = Items.LoadMoreItemsAsync(search.PerPage);
        }


    }
}
