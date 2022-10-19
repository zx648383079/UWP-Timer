using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Article
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListPage : Page
    {
        public ListPage()
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
