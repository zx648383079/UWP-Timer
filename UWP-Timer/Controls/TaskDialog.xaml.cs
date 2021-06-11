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
    public sealed partial class TaskDialog : ContentDialog
    {
        public TaskDialog()
        {
            InitializeComponent();
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
