using Microsoft.UI;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;

namespace ZoDream.LogTimer.Services
{
    public class ThemeService(ISettingService settings) : IThemeService
    {
        private readonly ISettingService _settings = settings;
        const string Key = "APPLICATION_KEY";

        public void SetTheme(string theme)
        {
            _settings.Set(Key, theme);
        }

        public static ApplicationTheme GetSystemThemeStatic()
        {
            var uiSettings = new Windows.UI.ViewManagement.UISettings();
            var color = uiSettings.GetColorValue(Windows.UI.ViewManagement.UIColorType.Background);
            return color == Colors.Black ? ApplicationTheme.Dark : ApplicationTheme.Light;
        }

        public static ApplicationTheme GetApplicationThemeStatic(string theme)
        {
            if (theme != ThemeConstants.System)
            {
                return theme == ThemeConstants.Dark ? ApplicationTheme.Dark : ApplicationTheme.Light;
            }
            else
            {
                return GetSystemThemeStatic();
            }
        }

        public ApplicationTheme GetSystemTheme()
        {
            return GetSystemThemeStatic();
        }

        public string GetTheme()
        {
            return _settings.Get<string>(Key);
        }

        public ApplicationTheme GetApplicationTheme()
        {

            return GetApplicationThemeStatic(GetTheme());
        }
    }
}
