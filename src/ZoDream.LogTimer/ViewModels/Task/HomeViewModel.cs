
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.LogTimer.Dialogs;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;
using ZoDream.LogTimer.Services;
using ZoDream.LogTimer.Utils;

namespace ZoDream.LogTimer.ViewModels
{
    public class HomeViewModel: ObservableObject
    {

        public HomeViewModel()
        {
            FastCommand = new RelayCommand(TapFast);
            search = new SearchForm()
            {
                Page = 0
            };
            Items = new IncrementalLoadingCollection<TaskDay>(count =>
            {
                return Task.Run(async () =>
                {
                    search.Page++;
                    var data = await _api.GetTaskDayListAsync(search);
                    if (data == null)
                    {
                        return Tuple.Create<IList<TaskDay>, bool>(null, false);
                    }
                    return Tuple.Create(data.Data, data.Paging.More);
                });
            });
        }
        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        private RestTaskRepository _api = App.GetService<RestTaskRepository>();
        private INotificationService _notify = App.GetService<INotificationService>();
        private SearchForm search;

        private IncrementalLoadingCollection<TaskDay> items;

        public IncrementalLoadingCollection<TaskDay> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        private ObservableCollection<TaskItem> taskItems = new ObservableCollection<TaskItem>();

        public ObservableCollection<TaskItem> TaskItems
        {
            get => taskItems;
            set => SetProperty(ref taskItems, value);
        }

        public ICommand FastCommand { get; private set; }

        private void TapFast()
        {
            _ = FastCreateAsync();
        }

        private async Task FastCreateAsync()
        {
            var dialog = new TaskDialog
            {
                Title = "快捷创建任务",
            };
            var result = await _app.OpenDialogAsync(dialog);
            if (result != ContentDialogResult.Primary)
            {
                return;
            }
            if (!dialog.CheckForm())
            {
                return;
            }
            var data = await _api.FastCreateAsync(dialog.FormData());
            if (data == null)
            {
                return;
            }
            _app.DispatcherQueue.TryEnqueue(() => {
                _notify.Success("添加成功");
                Items.Add(data);
            });
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
            var data = await _api.GetTaskAsync(new SearchForm()
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
            SynchronizationContext.Current!.Post(o =>
            {
                TaskItems.Clear();
                foreach (var item in data.Data)
                {
                    TaskItems.Add(item);
                }
            }, null);
        }

        public async Task AddToDay(TaskItem task)
        {
            var data = await _api.AddTodayTaskAsync(task.Id);
            if (data == null)
            {
                return;
            }
            SynchronizationContext.Current!.Post(o =>
            {
                _notify.Push("添加成功");
                foreach (var item in Items)
                {
                    if (item.Id == data.Id)
                    {
                        item.Amount = data.Amount;
                        return;
                    }
                }
                Items.Add(data);
            }, null);
        }

    }
}
