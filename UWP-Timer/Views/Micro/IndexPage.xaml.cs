using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Timer.Models;
using UWP_Timer.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP_Timer.Views.Micro
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
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
            NewBtn.Visibility = App.IsLogin? Visibility.Visible : Visibility.Collapsed;
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

        private void MicroViewer_ActionTapped(Controls.MicroViewer sender, ActionArgs<MicroItem> args)
        {
            if (args.Action == ActionType.COMMENT)
            {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("micro", sender);
                Frame.Navigate(typeof(DetailPage), sender.Source);
            }
        }
    }
}
