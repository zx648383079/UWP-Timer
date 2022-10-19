using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Extensions;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Utils;
using ZoDream.Shared.Http;

namespace ZoDream.LogTimer.ViewModels
{
    internal class MainViewModel: BindableBase
    {
        private bool _isLoading = false;

        /// <summary>
        /// Gets or sets a value indicating whether the Customers list is currently being updated. 
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private string Token { get; set; } = string.Empty;

        private User _user;

        public User User
        {
            get { return _user; }
            set
            {
                Set(ref _user, value);
                if (value != null)
                {
                    AppData.SetValue(Constants.USER_KEY, JsonConvert.SerializeObject(value));
                }

            }
        }


        private ObservableCollection<TabItem> tips = new();

        public ObservableCollection<TabItem> Tips
        {
            get { return tips; }
            set { Set(ref tips, value); }
        }

        public SettingItem GetSettings()
        {
            var str = AppData.GetValue<string>(Constants.SETTING_KEY);
            if (str == null)
            {
                return new SettingItem();
            }
            return JsonConvert.DeserializeObject<SettingItem>(str);
        }

        public void SetSettings(SettingItem item)
        {
            AppData.SetValue(Constants.SETTING_KEY, JsonConvert.SerializeObject(item));
        }

        public void LoadUser()
        {
            var str = AppData.GetValue<string>(Constants.USER_KEY);
            if (str == null)
            {
                return;
            }
            User = JsonConvert.DeserializeObject<User>(str);
        }

        public async Task LoadTipAsync(string keywords)
        {
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            var data = await App.Repository.Article.GetSuggestionAsync(keywords);

            await dispatcherQueue.EnqueueAsync(() =>
            {
                tips.Clear();
                foreach (var item in data.Data)
                {
                    Tips.Add(new TabItem(item.Id, item.Title));
                }

            });
        }

        public bool IsLogin
        {
            get
            {
                return !string.IsNullOrEmpty(Token);
            }
        }

        public void Login(User user)
        {
            Token = user.Token;
            User = user;
            AppData.SetValue(Constants.TOKEN_KEY, user.Token);
        }

        public void Logout()
        {
            Token = string.Empty;
            User = null;
            AppData.Remove(Constants.TOKEN_KEY);
            AppData.Remove(Constants.USER_KEY);
        }

        public async Task RefreshTokenAsync()
        {
            if (!IsLogin)
            {
                var token = AppData.GetValue<string>(Constants.TOKEN_KEY);
                if (string.IsNullOrWhiteSpace(token))
                {
                    return;
                }
                Token = token;
            }
            LoadUser();
            var user = await App.Repository.User.GetProfileAsync();
            if (user == null)
            {
                Logout();
                return;
            }
            User = user;
        }

        public void DefaultFailureRequest(HttpException ex)
        {
            if (ex.Code == 401)
            {
                Logout();
            }
            if (!string.IsNullOrWhiteSpace(ex.Message))
            {
                Toast.Tip(ex.Message);
            }
        }
    }
}
