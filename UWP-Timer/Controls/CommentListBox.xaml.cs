using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace UWP_Timer.Controls
{
    public sealed partial class CommentListBox : UserControl
    {
        public CommentListBox()
        {
            this.InitializeComponent();
        }

        public event TypedEventHandler<CommentItem, ActionArgs<CommentBase>> ActionTapped;

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

        private void RefreshView()
        {
            MainView.Children.Clear();
            if (Items == null)
            {
                return;
            }
            BindListener();
            foreach (var item in Items)
            {
                var control = new CommentItem()
                {
                    Source = item,
                };
                control.ActionTapped += Control_ActionTapped;
                MainView.Children.Add(control);
            }
        }

        private void Control_ActionTapped(CommentItem sender, ActionArgs<CommentBase> args)
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
