using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Timer.Models;
using UWP_Timer.Repositories;
using UWP_Timer.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP_Timer.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        public HomeViewModel ViewModel = new HomeViewModel();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            addBtn.Visibility = App.IsLogin? Visibility.Visible : Visibility.Collapsed;
            if (App.IsLogin)
            {
                ViewModel.Load();
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Tasks.IndexPage));
        }

        private void dayBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem as TaskDay;
            if (item == null)
            {
                return;
            }
            Frame.Navigate(typeof(Tasks.DetailPage), item.Id);
        }

        private void NewButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            splitView.IsPaneOpen = true;
        }

        private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            _ = ViewModel.LoadTask(args.QueryText);
        }

        private void AddToDay_Click(object sender, RoutedEventArgs e)
        {
            _ = ViewModel.AddToDay((sender as Button).DataContext as TaskItem);
        }

        private void ClosePanelBtn_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = false;
        }
    }
}
