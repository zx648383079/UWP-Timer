
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    public class ConnectViewModel: ObservableObject
    {

        public ConnectViewModel()
        {
            RefreshAsync();
        }

        private readonly RestAccountRepository _api = App.GetService<RestAccountRepository>();
        private readonly IAuthService _auth = App.GetService<IAuthService>();
        private readonly INotificationService _notify = App.GetService<INotificationService>();
        private ObservableCollection<Connect> items = [];

        public ObservableCollection<Connect> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        public async void RefreshAsync()
        {
            SynchronizationContext.Current?.Post(o => {
                _notify.Loading(true);
                Items.Clear();
            }, null);
            var data = await _api.GetConnectAsync(res =>
            {
                SynchronizationContext.Current?.Post(o =>
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
            SynchronizationContext.Current?.Post(o =>
             {
                 _notify.Loading(false);
                 foreach (var item in data.Data)
                 {
                     item.Label = item.Id > 0 ? $"已绑({item.Nickname})" : "未绑定";
                     item.Icon = Converters.ConverterHelper.Icon(item.Icon);
                     Items.Add(item);
                 }
             }, null);
        }

    }
}
