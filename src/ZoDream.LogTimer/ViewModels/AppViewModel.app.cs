using System.Collections.Generic;
using Newtonsoft.Json;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Utils;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    internal partial class AppViewModel
    {
        public AppOption SystemOption = new();

        public event SystemBootEventHandler? Booted;

        public async void LoadAsync()
        {
            var auth = App.GetService<IAuthService>();
            var api = App.GetService<RestSiteRepository>();
            auth.Authenticating = true;
            var data = await api.BatchAsync(new Dictionary<string, object>()
            {
                {"seo_configs", new { } },
                {"auth_profile", new { } },
            });
            if (data?.SeoConfigs is not null)
            {
                SystemOption = data.SeoConfigs;
            }
            if (data?.AuthProfile is not null)
            {
                auth.LoginAsync(data.AuthProfile);
            }
            else
            {
                auth.LogoutAsync();
            }
            Booted?.Invoke();
            if (SystemOption.SiteClose)
            {
                _ = ShowMessageAsync("当前应用不可用！");
            }
        }
    }
}
