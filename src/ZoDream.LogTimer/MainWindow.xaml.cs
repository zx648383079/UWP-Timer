using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using Windows.Storage;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Pages;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;
using ZoDream.LogTimer.Services;
using ZoDream.LogTimer.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            _navigation = App.GetService<NavigationService>();
            _navigation.RegisterFrame(AppFrame);
            SetTitleBar(AppTitleBar);
            OnLoad();
        }

        private NavigationService _navigation;
        private IAuthService _auth = App.GetService<IAuthService>();
        internal MainViewModel ViewModel = new();
        private bool isSearchChoose = false;// 阻止点击搜索项触发搜索

        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            // ApplicationView.GetForCurrentView().Title = App.GetString("app_name");
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
                label == "scanMenu" ? (_auth.Authenticated ? typeof(ScanPage) : typeof(Pages.Auth.LoginPage)) :
                label == "reviewMenu" ? typeof(Pages.Plan.ReviewPage) :
                label == "recordMenu" ? typeof(Pages.Plan.RecordPage) :
                label == "shareMenu" ? typeof(Pages.Plan.ShareListPage) :
                label == "taskMenu" ? typeof(Pages.Plan.IndexPage) :
                label == "microMenu" ? typeof(Pages.Micro.IndexPage) :
                label == "myMenu" ? typeof(Pages.Member.IndexPage) :
                typeof(Pages.Plan.TodayPage);
            if (pageType != null && pageType != AppFrame.CurrentSourcePageType)
            {
                _navigation.Navigate(pageType);
            }
        }

        private void OnLoad()
        {
            var app = App.GetService<AppViewModel>();
            app.BaseWindow = this;
            _navigation.Navigate(typeof(Pages.Plan.TodayPage));
            AppTitle.Text = App.GetString("app_name");
            app.Booted += () => {
                if (_auth.Authenticated)
                {
                    _navigation.Navigate(AppFrame.SourcePageType);
                    AppFrame.BackStack.Clear();
                }
            };
            _auth.AuthChanged += () => {
                DispatcherQueue.TryEnqueue(() => {
                    myMenu.Content = _auth.Authenticated ? 
                    _auth.AuthenticatedUser!.Name : App.GetString("login_btn_label");
                });
            };
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

        private void BackBtn_Click(object sender, RoutedEventArgs e)
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
            AppFrame.Navigate(typeof(Pages.Article.DetailPage), s.Value);
            //search(s);
        }

        private void search(string keywords)
        {
            if (string.IsNullOrWhiteSpace(keywords))
            {
                return;
            }
            AppFrame.Navigate(typeof(Pages.Article.ListPage), new SearchForm() { Keywords = keywords });
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
            AppFrame.Navigate(typeof(Pages.Member.IndexPage));
        }

        internal void NavigateWithDeepLink(Uri uri)
        {
            App.GetService<Deeplink>().OpenLink(uri);
        }

        internal void NavigateWithFile(IReadOnlyList<IStorageItem> files)
        {
            var share = App.GetService<ShareService>();
            foreach (var item in files)
            {
                _ = share.DecodeAsync(item as IStorageFile);
                return;
            }
        }

        private void Window_Closed(object sender, WindowEventArgs args)
        {
            App.GetService<AppViewModel>()?.Dispose();
        }
    }
}
