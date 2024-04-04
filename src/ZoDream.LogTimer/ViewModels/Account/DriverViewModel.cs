using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    public class DriverViewModel: ObservableObject
    {
        public DriverViewModel()
        {
            RefreshAsync();
        }

        private readonly IAuthService _auth = App.GetService<IAuthService>();
        private readonly RestAccountRepository _api = App.GetService<RestAccountRepository>();
        private readonly INotificationService _notify = App.GetService<INotificationService>();
        private ObservableCollection<Driver> items = [];

        public ObservableCollection<Driver> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        public async void RefreshAsync()
        {
            SynchronizationContext.Current!.Post(o => {
                _notify.Loading(true);
                Items.Clear();
            }, null);
            var data = await _api.GetDriverAsync(res =>
            {
                SynchronizationContext.Current.Post(o =>
                {
                    _notify.Loading(false);
                    if (res.Code == 401)
                    {
                        _auth.LogoutAsync();
                    }
                }, null);

            });
            if (data == null || data.Data == null)
            {
                return;
            }
            SynchronizationContext.Current.Post(o =>
            {
                _notify.Loading(false);
                foreach (var item in data.Data)
                {
                    Items.Add(item);
                }
            }, null);
        }
    }
}
