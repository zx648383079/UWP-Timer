using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using ZoDream.LogTimer.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    public sealed class CommentListBox : Control
    {
        public CommentListBox()
        {
            this.DefaultStyleKey = typeof(CommentListBox);
        }

        public event TypedEventHandler<CommentListItem, ActionArgs<CommentBase>> ActionTapped;

        public IEnumerable<CommentBase> Items
        {
            get { return (IEnumerable<CommentBase>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable<CommentBase>), typeof(CommentListBox), new PropertyMetadata(null, OnItemsChange));

        private static void OnItemsChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // (d as CommentListBox).RefreshView();
        }
    }
}
