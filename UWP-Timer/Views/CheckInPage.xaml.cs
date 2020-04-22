using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class CheckInPage : Page
    {
        public CheckInPage()
        {
            this.InitializeComponent();
        }

        public CheckInViewModel ViewModel = new CheckInViewModel();


        private void PreviousBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.PreviousMonth();
        }

        private void NextBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.NextMonth();
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _ = ViewModel.CheckTodayAsync();
        }
    }
}
