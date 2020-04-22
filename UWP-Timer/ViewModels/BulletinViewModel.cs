using System;
using System.Collections.Generic;
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
            Items = new IncrementalLoadingCollection<BulletinUser>(count =>
            {
                return Task.Run(async () =>
                {
                    if (!App.IsLogin())
                    {
                        return Tuple.Create<IList<BulletinUser>, bool>(null, false);
                    }
                    search.Page++;
                    var data = await App.Repository.Bulletin.GetBulletinListAsync(search);
                    if (data == null)
                    {
                        return Tuple.Create<IList<BulletinUser>, bool>(null, false);
                    }
                    return Tuple.Create(data.Data, data.Paging.More);
                });
            });
            Load();
        }

        private SearchForm search;

        private IncrementalLoadingCollection<BulletinUser> items;

        public IncrementalLoadingCollection<BulletinUser> Items
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
            items.Clear();
            _ = Items.LoadMoreItemsAsync(search.PerPage);
        }

    }
}
