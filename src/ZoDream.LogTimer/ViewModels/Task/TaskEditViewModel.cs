using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.LogTimer.Dialogs;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;
using ZoDream.LogTimer.Services;
using ZoDream.LogTimer.Utils;

namespace ZoDream.LogTimer.ViewModels
{
    internal class TaskEditViewModel : ObservableObject
    {
        public TaskEditViewModel()
        {
            SubmitCommand = new RelayCommand(TapSubmit);
            AddCommand = new RelayCommand(TapAdd);
            ShareCommand = new RelayCommand(TapShare);
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        private readonly INotificationService _notify = App.GetService<INotificationService>();
        private readonly NavigationService _router = App.GetService<NavigationService>();
        private readonly RestTaskRepository _api = App.GetService<RestTaskRepository>();
        private readonly IAuthService _auth = App.GetService<IAuthService>();
        private int Id;

        private string name = string.Empty;

        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string description = string.Empty;

        public string Description {
            get => description;
            set => SetProperty(ref description, value);
        }

        private int everyTime = 25;

        public int EveryTime {
            get => everyTime;
            set => SetProperty(ref everyTime, value);
        }

        private int spaceTime;

        public int SpaceTime {
            get => spaceTime;
            set => SetProperty(ref spaceTime, value);
        }

        private DateTime startAt;

        public DateTime StartAt {
            get => startAt;
            set => SetProperty(ref startAt, value);
        }

        private int perTime;

        public int PerTime {
            get => perTime;
            set => SetProperty(ref perTime, value);
        }


        public string Title => App.GetString(Id > 0 ? "task_edit_title" : "task_new_title");

        public bool AddEnabled => Id > 0;

        public ICommand SubmitCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand ShareCommand { get; private set; }

        private void TapSubmit()
        {
            var form = new TaskForm()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                EveryTime = EveryTime,
            };
            if (string.IsNullOrWhiteSpace(form.Name))
            {
                _notify.Warning(App.GetString("task_name.PlaceholderText"));
                return;
            }
            _ = SaveAsync(form);
        }

        private void TapAdd()
        {
            _ = ShowChildAsync();
        }

        private void TapShare()
        {
            _ = ShowShareAsync();
        }

        private async Task SaveAsync(TaskForm form)
        {
            _notify.Loading(true);
            var data = await _api.SaveTaskAsync(form, res => {
                _app.DispatcherQueue.TryEnqueue(() => {
                    _notify.Error(res.Message);
                });
            });
            _app.DispatcherQueue.TryEnqueue(() => {
                _notify.Loading(false);
                _notify.Success(App.GetString("task_save_success"));
                _router.GoBack();
            });
        }


        private async Task ShowChildAsync()
        {
            var dialog = new TaskDialog();
            var result = await _app.OpenDialogAsync(dialog);
            if (result != ContentDialogResult.Primary)
            {
                return;
            }
            if (!dialog.CheckForm())
            {
                return;
            }
            var item = dialog.FormData();
            item.ParentId = Id;
            var data = await _api.SaveTaskAsync(item);
            if (data == null)
            {
                return;
            }
            _notify.Success("添加成功");
        }

        private async Task ShowShareAsync()
        {
            var dialog = new TaskShareDialog();
            var result = await _app.OpenDialogAsync(dialog);
            if (result != ContentDialogResult.Primary)
            {
                return;
            }
            var item = dialog.FormData();
            item.TaskId = Id;
            var data = await _api.ShareCreateAsync(item);
            if (data == null)
            {
                return;
            }
            _notify.Success("分享成功");
            _app.DispatcherQueue.TryEnqueue(() => {
                // 显示分享链接/二维码
                _ = dialog.ShowAsync();
            });
        }

        private async Task LoadTaskAsync()
        {
            _notify.Loading(true);
            var data = await _api.GetTaskDetailAsync(Id);
            _app.DispatcherQueue.TryEnqueue(() => {
                _notify.Loading(false);
                if (data == null)
                {
                    _router.GoBack();
                    return;
                }
                Id = data.Id;
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(AddEnabled));
                Name = data.Name;
                Description = data.Description;
                EveryTime = data.EveryTime;
                SpaceTime = data.SpaceTime;
                PerTime = data.PerTime;
                StartAt = Time.TimestampTo(data.StartAt);
            });
        }


        public void Load(int id)
        {
            Id = id;
            if (Id > 0)
            {
                _ = LoadTaskAsync();
            }
        }
    }
}
