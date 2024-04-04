
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;
using ZoDream.LogTimer.Services;
using ZoDream.LogTimer.Utils;
using static Vanara.PInvoke.ComCtl32;

namespace ZoDream.LogTimer.ViewModels
{
    public class MicroDetailViewModel : ObservableObject
    {
        public MicroDetailViewModel()
        {
            CommentCommand = new RelayCommand(TapComment);
            ReplyCommand = new RelayCommand<CommentBase>(TapReply);
        }
        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        private RestMicroRepository _api = App.GetService<RestMicroRepository>();
        private INotificationService _notify = App.GetService<INotificationService>();
        private readonly NavigationService _router = App.GetService<NavigationService>();
        private readonly IAuthService _auth = App.GetService<IAuthService>();

        private MicroItem data = null;

        public MicroItem Data
        {
            get => data;
            set => SetProperty(ref data, value);
        }


        private User user = new();

        public User User
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        private ObservableCollection<CommentBase> commentItems = [];

        public ObservableCollection<CommentBase> CommentItems
        {
            get => commentItems;
            set => SetProperty(ref commentItems, value);
        }

        public MicroCommentQueries Queries { get; private set; } = new();
        public bool IsLoading { get; private set; } = false;
        public bool HasMore { get; private set; } = true;

        private int ParentId = 0;

        private string content;

        public string Content {
            get => content;
            set => SetProperty(ref content, value);
        }

        private bool commentEnabled;

        public bool CommentEnabled {
            get => commentEnabled;
            set => SetProperty(ref commentEnabled, value);
        }

        private bool isForward;

        public bool IsForward {
            get => isForward;
            set => SetProperty(ref isForward, value);
        }


        public ICommand CommentCommand { get; private set; }
        public ICommand ReplyCommand { get; private set; }

        private void TapReply(CommentBase? target)
        {
            if (target is null)
            {
                return;
            }
            ParentId = target.Id;
            if (target.User != null)
            {
                Content += $"@{target.User.Name} ";
            }
        }

        private void TapComment()
        {
            if (string.IsNullOrWhiteSpace(Content))
            {
                _notify.Warning("请输入内容");
                return;
            }
            CommentEnabled = false;
            _ = CreateAsync(new MicroCommentForm()
            {
                MicroId = Data.Id,
                ParentId = ParentId,
                Conent = Content,
                IsForward = IsForward
            });
        }

        private async Task CreateAsync(MicroCommentForm form)
        {
            var data = await _api.CreateCommentAsync(form);
            if (data == null)
            {
                return;
            }
            _app.DispatcherQueue.TryEnqueue(() => {
                _notify.Success("评论成功");
                Content = "";
                IsForward = false;
                ParentId = 0;
            });
        }

        public async Task TapRefreshAsync()
        {
            await LoadPageAsync(1);
        }

        public async Task TapMoreAsync()
        {
            if (!HasMore)
            {
                return;
            }
            await LoadPageAsync(Queries.Page + 1);
        }

        public async Task TapPageAsync()
        {
            await LoadPageAsync(Queries.Page);
        }

        public async Task LoadPageAsync(uint page)
        {
            if (IsLoading)
            {
                return;
            }
            IsLoading = true;
            Queries.Id = Data.Id;
            if (Queries.Page <= page)
            {
                CommentItems.Clear();
            }
            Queries.Page = page;
            var data = await _api.GetCommentAsync(Queries);
            IsLoading = false;
            if (data == null)
            {
                return;
            }
            HasMore = data.Paging.More;
            _app.DispatcherQueue.TryEnqueue(() => {
                foreach (var item in data.Data)
                {
                    CommentItems.Add(item);
                }
            });
        }


        public async Task LoadDetailAsync(int id)
        {
            _notify.Loading(true);
            var data = await _api.GetAsync(id);
            _app.DispatcherQueue.TryEnqueue(() => {
                _notify.Loading(false);
                if (data == null)
                {
                    _router.GoBack();
                    return;
                }
                SetMicro(data);
            });
        }

        private void SetMicro(MicroItem data)
        {
            Data = data;
            _ = TapRefreshAsync();
        }

        public void Load(MicroItem data)
        {
            if (string.IsNullOrEmpty(data.Content))
            {
                _ = LoadDetailAsync(data.Id);
            }
            else
            {
                SetMicro(data);
            }
            if (_auth.Authenticated)
            {
                User = _auth.AuthenticatedUser;
            }
        }
    }
}
