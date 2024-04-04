using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Services;
using ZoDream.LogTimer.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Micro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IndexPage : Page
    {
        public IndexPage()
        {
            this.InitializeComponent();
        }

        public MicroViewModel ViewModel = new MicroViewModel();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var auth = App.GetService<IAuthService>();
            NewBtn.Visibility = auth.Authenticated ? Visibility.Visible : Visibility.Collapsed;
        }

        private void statusBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var status = (int)(statusBar.SelectedItem as TabItem).Value;
            ViewModel.Load(status);
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PublishPage));
        }

        private void MicroViewer_ActionTapped(Controls.MicroListItem sender, ActionArgs<MicroItem> args)
        {
            if (args.Action == ActionType.COMMENT)
            {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("micro", sender);
                Frame.Navigate(typeof(DetailPage), sender.Source);
            }
        }

        private void MicroListItem_RuleTapped(Controls.MicroListItem sender, Controls.RuleTappedArgs args)
        {
            if (args.Block.Type == BlockType.LINK)
            {
                App.GetService<Deeplink>().OpenLink(args.Block.Value.ToString());
            }
        }
    }
}
