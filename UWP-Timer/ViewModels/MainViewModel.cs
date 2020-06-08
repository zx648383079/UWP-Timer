using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;
using UWP_Timer.Repositories;
using UWP_Timer.Utils;

namespace UWP_Timer.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public MainViewModel()
        {

        }
        private bool _isLoading = false;

        /// <summary>
        /// Gets or sets a value indicating whether the Customers list is currently being updated. 
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private User _user;

        public User User
        {
            get { return _user;}
            set {
                Set(ref _user, value);
                if (value != null)
                {
                    AppData.SetValue(Constants.USER_KEY, JsonConvert.SerializeObject(value));
                }

            }
        }


        private ObservableCollection<TabItem> tips = new ObservableCollection<TabItem>();

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
            var data = await App.Repository.Article.GetSuggestionAsync(keywords);

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                tips.Clear();
                foreach (var item in data.Data)
                {
                    Tips.Add(new TabItem(item.Id, item.Title));
                }

            });
        }
    }
}
