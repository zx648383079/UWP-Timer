
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories.Models;

namespace ZoDream.LogTimer.ViewModels
{
    public class MicroDetailViewModel : BindableBase
    {
        public MicroDetailViewModel()
        {
        }

        private MicroItem data = null;

        public MicroItem Data
        {
            get => data;
            set => Set(ref data, value);
        }


        private User user = new User();

        public User User
        {
            get => user;
            set => Set(ref user, value);
        }

        private ObservableCollection<CommentBase> commentItems = new ObservableCollection<CommentBase>();

        public ObservableCollection<CommentBase> CommentItems
        {
            get => commentItems;
            set => Set(ref commentItems, value);
        }

        public MicroCommentQueries Queries { get; private set; } = new MicroCommentQueries();
        public bool IsLoading { get; private set; } = false;
        public bool HasMore { get; private set; } = true;

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
            var data = await App.Repository.Micro.GetCommentAsync(Queries);
            IsLoading = false;
            if (data == null)
            {
                return;
            }
            HasMore = data.Paging.More;
            SynchronizationContext.Current.Post(o =>
            {
                foreach (var item in data.Data)
                {
                    CommentItems.Add(item);
                }
            }, null);
        }
        
    }
}
