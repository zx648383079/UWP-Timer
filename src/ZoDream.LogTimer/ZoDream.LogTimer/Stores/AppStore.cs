using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Utils;

namespace ZoDream.LogTimer.Stores
{
    internal class AppStore
    {

        public AppOption SystemOption = new();

        public UserOption UserOption = new();

        public AuthStore Auth = new();


        public async void LoadAsync()
        {
            var str = AppData.GetValue<string>(Constants.SETTING_KEY);
            if (str != null)
            {
                UserOption = JsonConvert.DeserializeObject<UserOption>(str);
            }
            var data = await App.Repository.Site.BatchAsync(new Dictionary<string, object>()
            {
                {"seo_configs", new { } },
                {"auth_profile", new { } },
            });
            if (data.SeoConfigs is not null)
            {
                SystemOption = data.SeoConfigs;
            }
            if (data.AuthProfile is not null)
            {
                Auth.LoginAsync(data.AuthProfile);
            } else
            {
                Auth.LogoutAsync();
            }
        }

        public void SaveAsync(UserOption option)
        {
            UserOption = option;
            SaveAsync();
        }

        public void SaveAsync() 
        {
            AppData.SetValue(Constants.SETTING_KEY, JsonConvert.SerializeObject(UserOption));
        }
    }
}
