using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Services
{
    public interface IThemeService
    {
        public void SetTheme(string theme);
        public ApplicationTheme GetSystemTheme();
        public ApplicationTheme GetApplicationTheme();
        public string GetTheme();
    }
}
