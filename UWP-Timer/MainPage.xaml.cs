using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Timer.Models;
using UWP_Timer.Repositories;
using UWP_Timer.ViewModels;
using UWP_Timer.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace UWP_Timer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public Frame AppFrame => frame;
        public MainViewModel ViewModel = App.ViewModel;
        private bool isSearchChoose = false;// 阻止点击搜索项触发搜索

        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            // ApplicationView.GetForCurrentView().Title = Constants.GetString("app_name");
            if (e.NavigationMode == NavigationMode.Back)
            {
                if (e.SourcePageType == typeof(SettingPage))
                {
                    NavView.SelectedItem = NavView.SettingsItem;
                }
            }
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var label = args.InvokedItemContainer.Name;
            var pageType =
                args.IsSettingsInvoked ? typeof(SettingPage) :
                label == "scanMenu" ? (App.IsLogin() ? typeof(ScanPage) : typeof(Views.Member.LoginPage)) :
                label == "reviewMenu" ? typeof(Views.Review.IndexPage) :
                label == "recordMenu" ? typeof(Views.Review.RecordPage) :
                label == "shareMenu" ? typeof(Views.Share.IndexPage) :
                label == "taskMenu" ? typeof(Views.Tasks.IndexPage) :
                label == "microMenu" ? typeof(Views.Micro.IndexPage) :
                label == "myMenu" ? typeof(Views.Member.IndexPage) :
                typeof(HomePage);
            if (pageType != null && pageType != AppFrame.CurrentSourcePageType)
            {
                AppFrame.Navigate(pageType);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                AppFrame.Navigate(typeof(HomePage));
            }
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Navigates the frame to the previous page.
        /// </summary>
        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (AppFrame.CanGoBack)
            {
                AppFrame.GoBack();
            }
        }

        private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (isSearchChoose)
            {
                isSearchChoose = false;
                return;
            }
            search(args.QueryText);
        }

        private void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            isSearchChoose = true;
            var s = args.SelectedItem as TabItem;
            // 将从下拉列表中选择的项放入输入框
            sender.Text = s == null ? string.Empty : s.Name;
            AppFrame.Navigate(typeof(Views.Article.DetailPage), s.Value);
            //search(s);
        }

        private void search(string keywords)
        {
            if (string.IsNullOrWhiteSpace(keywords))
            {
                return;
            }
            AppFrame.Navigate(typeof(Views.Article.IndexPage), new SearchForm() { Keywords = keywords });
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (isSearchChoose)
            {
                return;
            }
            _ = ViewModel.LoadTipAsync(sender.Text);
        }

        private void NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AppFrame.Navigate(typeof(Views.Member.IndexPage));
        }
    }
}
