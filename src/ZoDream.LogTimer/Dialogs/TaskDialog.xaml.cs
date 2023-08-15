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
using ZoDream.LogTimer.Repositories.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Dialogs
{
    public sealed partial class TaskDialog : ContentDialog
    {
        public TaskDialog()
        {
            this.InitializeComponent();
        }

        private int id = 0;

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (!CheckForm())
            {
                args.Cancel = true;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        internal bool CheckForm()
        {
            return !string.IsNullOrWhiteSpace(nameTb.Text);
        }

        public TaskItem Source
        {
            set
            {
                id = value.Id;
                nameTb.Text = value.Name;
                descTb.Text = value.Description;
            }
        }

        internal TaskForm FormData()
        {
            return new TaskForm()
            {
                Id = id,
                Name = nameTb.Text.Trim(),
                Description = descTb.Text.Trim()
            };
        }
    }
}
