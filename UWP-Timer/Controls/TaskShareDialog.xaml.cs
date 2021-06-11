using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Timer.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace UWP_Timer.Controls
{
    public sealed partial class TaskShareDialog : ContentDialog
    {
        public TaskShareDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        internal TaskShareForm FormData()
        {
            return new TaskShareForm()
            {
                ShareType = typeCb.SelectedIndex,
                ShareRule = pwdTb.Text,
            };
        }

        private void typeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pwdBox.Visibility = typeCb.SelectedIndex < 1 ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
