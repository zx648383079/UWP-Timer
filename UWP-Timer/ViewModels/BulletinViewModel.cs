using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;

namespace UWP_Timer.ViewModels
{
    public class BulletinViewModel: BindableBase
    {
        public BulletinViewModel()
        {
            search = new SearchForm()
            {
                Page = 0
            };
            //Items = new IncrementalLoadingCollection<Bulletin>(count =>
            //{
            //    return Task.Run(async () =>
            //    {
            //        if (!App.IsLogin())
            //        {
            //            return Tuple.Create<IList<Bulletin>, bool>(null, false);
            //        }
            //        search.Page++;
            //        var data = await App.Repository.Bulletin.GetBulletinListAsync(search);
            //        if (data == null)
            //        {
            //            return Tuple.Create<IList<Bulletin>, bool>(null, false);
            //        }
            //        return Tuple.Create(data.Data, data.Paging.More);
            //    });
            //});
            Load();
        }

        private SearchForm search;

        private ObservableCollection<MessageBase> items = new ObservableCollection<MessageBase>();

        public ObservableCollection<MessageBase> Items
        {
            get { return items; }
            set { Set(ref items, value); }
        }

        private ObservableCollection<UserItem> userItems = new ObservableCollection<UserItem>();

        public ObservableCollection<UserItem> UserItems
        {
            get => userItems;
            set => Set(ref userItems, value);
        }


        public void Load()
        {
            
            // Refresh();
            _ = loadUserAsync();
        }


        private async Task loadUserAsync()
        {
            var data = await App.Repository.Bulletin.GetUserListAsync();
            if (data == null)
            {
                return;
            }
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            await dispatcherQueue.EnqueueAsync(() =>
            {
                UserItems.Clear();
                foreach (var item in data.Data)
                {
                    UserItems.Add(item);
                }
            });
        }

        public void Refresh()
        {
            search.Page = 0;
            Items.Clear();
            _ = LoadItemsAsync();//Items.LoadMoreItemsAsync(search.PerPage);
        }


        internal void LoadType(int v)
        {
            search.Type = v.ToString();
            search.User = 0;
            Refresh();
        }

        internal void LoadUser(int v)
        {
            if (v == 0)
            {
                v = -1;
            }
            search.Type = null;
            search.User = v;
            Refresh();
        }

        public async Task LoadItemsAsync()
        {
            Items.Clear();
            var data = await App.Repository.Bulletin.GetBulletinListAsync(search);
            if (data == null)
            {
                return;
            }
            for (int i = data.Data.Count - 1; i >= 0; i--)
            {
                var item = data.Data[i];
                if (item.Bulletin == null)
                {
                    continue;
                }
                Items.Add(item.Bulletin.ToMessage());
            }
        }
    }
}
