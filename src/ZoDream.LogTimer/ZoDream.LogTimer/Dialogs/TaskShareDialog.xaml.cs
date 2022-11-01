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
using ZoDream.LogTimer.Repositories.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Dialogs
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
