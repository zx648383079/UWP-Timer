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
    public class TaskViewModel: ObservableObject
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
                    var data = await _api.GetTaskAsync(search);
                    if (data == null)
                    {
                        return Tuple.Create<IList<TaskItem>, bool>(null, false);
                    }
                    return Tuple.Create(data.Data, data.Paging.More);
                });
            });
            StatusItems.Add(new TabItem(1, App.GetString("task_tab_1")));
            StatusItems.Add(new TabItem(2, App.GetString("task_tab_2")));
        }
        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        private readonly RestTaskRepository _api = App.GetService<RestTaskRepository>();
        private readonly INotificationService _notify = App.GetService<INotificationService>();
        private readonly IAuthService _auth = App.GetService<IAuthService>();
        private SearchForm search;

        public bool IsAuthenticated => _auth.Authenticated;

        private bool inEdit = false;

        public bool InEdit
        {
            get => inEdit;
            set => SetProperty(ref inEdit, value);
        }

        private ObservableCollection<TabItem> statusItems = new ObservableCollection<TabItem>();

        public ObservableCollection<TabItem> StatusItems
        {
            get => statusItems;
            set => SetProperty(ref statusItems, value);
        }


        private IncrementalLoadingCollection<TaskItem> items;

        public IncrementalLoadingCollection<TaskItem> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        public async Task StopTaskAsync(int[] items)
        {
            _notify.Loading(true);
            var data = await _api.BatchStopTaskAsync(items);
            _app.DispatcherQueue.TryEnqueue(() => {
                _notify.Loading(false);
                if (data == null)
                {
                    return;
                }
                _notify.Success(App.GetString("task_stop_success"));
                Refresh();
            });
        }

        public async Task AddTaskAsync(int[] items)
        {
            _notify.Loading(true);
            var data = await _api.BatchAddTaskAsync(items);
            _app.DispatcherQueue.TryEnqueue(() => {
                _notify.Loading(false);
                if (data == null)
                {
                    return;
                }
                _notify.Success(App.GetString("task_add_today_success"));
            });
        }

        public void Warning(string message)
        {
            _notify.Warning(message);
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
