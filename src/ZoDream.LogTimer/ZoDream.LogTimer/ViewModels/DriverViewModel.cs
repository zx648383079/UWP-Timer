
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;

namespace ZoDream.LogTimer.ViewModels
{
    public class DriverViewModel: BindableBase
    {
        public DriverViewModel()
        {
            RefreshAsync();
        }

        private ObservableCollection<Driver> items = new ObservableCollection<Driver>();

        public ObservableCollection<Driver> Items
        {
            get => items;
            set => Set(ref items, value);
        }

        public async void RefreshAsync()
        {
            SynchronizationContext.Current.Post(o => {
                App.ViewModel.IsLoading = true;
                Items.Clear();
            }, null);
            var data = await App.Repository.Account.GetDriverAsync(res =>
            {
                SynchronizationContext.Current.Post(o =>
                {
                    App.ViewModel.IsLoading = false;
                    if (res.Code == 401)
                    {
                        App.Store.Auth.LogoutAsync();
                    }
                }, null);

            });
            if (data == null || data.Data == null)
            {
                return;
            }
            SynchronizationContext.Current.Post(o =>
            {
                App.ViewModel.IsLoading = false;
                foreach (var item in data.Data)
                {
                    Items.Add(item);
                }
            }, null);
        }
    }
}
