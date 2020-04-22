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
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP_Timer.Views.Article
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

        public ArticleViewModel ViewModel = new ArticleViewModel();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter == null)
            {
                return;
            }
            var form = (SearchForm)e.Parameter;
            ViewModel.Search(form);
        }

        private void articleListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem as Models.Article;
            Frame.Navigate(typeof(DetailPage), item.Id);
        }

        private void sortBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.ChangeSort((string)((sender as ListBox).SelectedItem as TabItem).Value);
        }

        private void catBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.ChangeCategory((int)((sender as ListBox).SelectedItem as TabItem).Value);
        }
    }
}
