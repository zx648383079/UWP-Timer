using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;
using UWP_Timer.Utils;

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

        private ObservableCollection<TaskItem> taskItems = new ObservableCollection<TaskItem>();

        public ObservableCollection<TaskItem> TaskItems
        {
            get => taskItems;
            set => Set(ref taskItems, value);
        }


        public void Load()
        {
            Refresh();
        }

        public void Refresh()
        {
            search.Page = 0;
            Items.Clear();
            _ = Items.LoadMoreItemsAsync(search.PerPage);
        }


        public async Task LoadTask(string keywords)
        {
            var data = await App.Repository.Task.GetTaskAsync(new SearchForm()
            {
                Keywords = keywords,
                Page = 1,
                PerPage = 20,
                Status = 0
            });
            if (data == null)
            {
                return;
            }
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            await dispatcherQueue.EnqueueAsync(() =>
            {
                TaskItems.Clear();
                foreach (var item in data.Data)
                {
                    TaskItems.Add(item);
                }
            });
        }

        public async Task AddToDay(TaskItem task)
        {
            var data = await App.Repository.Task.AddTodayTaskAsync(task.Id);
            if (data == null)
            {
                return;
            }
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            await dispatcherQueue.EnqueueAsync(() =>
            {
                Toast.Tip("添加成功");
                foreach (var item in Items)
                {
                    if (item.Id == data.Id)
                    {
                        item.Amount = data.Amount;
                        return;
                    }
                }
                Items.Add(data);
            });
        }

    }
}
