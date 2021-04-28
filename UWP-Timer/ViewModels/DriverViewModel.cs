using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;

namespace UWP_Timer.ViewModels
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
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            await dispatcherQueue.EnqueueAsync(() => {
                App.ViewModel.IsLoading = true;
                Items.Clear();
            });
            var data = await App.Repository.Account.GetDriverAsync(async res =>
            {
                await dispatcherQueue.EnqueueAsync(() =>
                {
                    App.ViewModel.IsLoading = false;
                    if (res.Code == 401)
                    {
                        App.Logout();
                    }
                });

            });
            if (data == null || data.Data == null)
            {
                return;
            }
            await dispatcherQueue.EnqueueAsync(() =>
            {
                App.ViewModel.IsLoading = false;
                foreach (var item in data.Data)
                {
                    Items.Add(item);
                }
            });
        }
    }
}
