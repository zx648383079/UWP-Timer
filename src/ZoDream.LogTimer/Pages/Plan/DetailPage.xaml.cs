using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Threading.Tasks;
using Windows.System.Display;
using ZoDream.LogTimer.Controls;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Utils;
using ZoDream.LogTimer.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Plan
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        public DetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Load((int)e.Parameter);
            ToggleMiniBtn.Visibility = ViewModel.MiniEnabled ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            ViewModel.Dispose();
        }


        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            progressBar.Dispose();
            progressBar = null;
        }

        private void openComment_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = true;
        }

        private void ClosePanelBtn_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = false;
        }

    }
}
