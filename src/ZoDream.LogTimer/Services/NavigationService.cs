using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Pages;
using ZoDream.LogTimer.Pages.Plan;

namespace ZoDream.LogTimer.Services
{
    public class NavigationService
    {

        private Frame? _rootFrame;
        private BrowserWindow? _browser;
        private MiniTimerPage? _miniTimer;

        public void RegisterFrame(Frame frame)
        {
            _rootFrame = frame;
        }

        public void Navigate(Type pageType)
        {
            _rootFrame?.Navigate(pageType);
        }

        public void Navigate(Type pageType, object parameter)
        {
            _rootFrame?.Navigate(pageType, parameter);
        }

        public void GoBack()
        {
            _rootFrame?.GoBack();
        }



        public void ShowTimer()
        {
            _miniTimer ??= new MiniTimerPage();
            _miniTimer.Activate();
        }

        public void CloseTimer()
        {
            _miniTimer?.Close();
            _miniTimer = null;
        }

        public void NavigateUrl(Uri url)
        {
            _browser ??= new BrowserWindow();
            _browser.Activate();
            _browser.NavigateUrl(url);
        }
    }
}
