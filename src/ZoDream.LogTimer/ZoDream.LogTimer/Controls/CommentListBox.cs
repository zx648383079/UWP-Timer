using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
    [TemplatePart(Name = MainPanelName, Type = typeof(Panel))]
    public sealed class CommentListBox : Control
    {
        const string MainPanelName = "PART_MainPanel";
        public CommentListBox()
        {
            this.DefaultStyleKey = typeof(CommentListBox);
        }

        public event TypedEventHandler<CommentListItem, ActionArgs<CommentBase>> ActionTapped;

        private Panel MainPanel;

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
            (d as CommentListBox).RefreshView();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            MainPanel = GetTemplateChild(MainPanelName) as Panel;
        }

        private void RefreshView()
        {
            MainPanel.Children.Clear();
            if (Items == null)
            {
                return;
            }
            BindListener();
            foreach (var item in Items)
            {
                var control = new CommentListItem()
                {
                    Source = item,
                };
                control.ActionTapped += Control_ActionTapped;
                MainPanel.Children.Add(control);
            }
        }

        private void Control_ActionTapped(CommentListItem sender, ActionArgs<CommentBase> args)
        {
            ActionTapped?.Invoke(sender, args);
        }

        private void BindListener()
        {
            if (Items == null)
            {
                return;
            }
            if (Items is INotifyCollectionChanged)
            {
                var obj = Items as INotifyCollectionChanged;
                obj.CollectionChanged -= Obj_CollectionChanged;
                obj.CollectionChanged += Obj_CollectionChanged;
            }
            if (Items is INotifyPropertyChanged)
            {
                var obj = Items as INotifyPropertyChanged;
                obj.PropertyChanged -= Obj_PropertyChanged;
                obj.PropertyChanged += Obj_PropertyChanged;
            }
        }

        private void Obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RefreshView();
        }

        private void Obj_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshView();
        }
    }
}
