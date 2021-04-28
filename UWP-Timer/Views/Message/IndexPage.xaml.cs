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

namespace UWP_Timer.Views.Message
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

        public BulletinViewModel ViewModel = new BulletinViewModel();

        private void commentBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadType(8);
        }

        private void atBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadType(7);
        }

        private void agreeBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadType(6);
        }

        private void systemBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadUser(0);
        }

        private void userList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.LoadUser(((sender as ListView).SelectedItem as UserItem).Id);
        }
    }
}
