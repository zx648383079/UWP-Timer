using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    [TemplatePart(Name = ScrollBarName, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = InnerPanelName, Type = typeof(StackPanel))]
    public sealed class MessageContainer : Control
    {
        const string ScrollBarName = "PART_ScrollBar";
        const string InnerPanelName = "PART_InnerPanel";
        public MessageContainer()
        {
            this.DefaultStyleKey = typeof(MessageContainer);
        }

        private DispatcherTimer timer;
        private ScrollViewer ScrollBar;
        private StackPanel InnerPanel;

        public uint Sender
        {
            get { return (uint)GetValue(SenderProperty); }
            set { SetValue(SenderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Sender.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SenderProperty =
            DependencyProperty.Register("Sender", typeof(uint), typeof(MessageContainer), new PropertyMetadata(0));



        public int MaxTime
        {
            get { return (int)GetValue(MaxTimeProperty); }
            set { SetValue(MaxTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxTimeProperty =
            DependencyProperty.Register("MaxTime", typeof(int), typeof(MessageContainer), new PropertyMetadata(600));



        public IEnumerable<MessageBase> Items
        {
            get { return (IEnumerable<MessageBase>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable<MessageBase>), typeof(MessageContainer), new PropertyMetadata(null, new PropertyChangedCallback(OnItemsChange)));

        private static void OnItemsChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MessageContainer).RefreshView();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ScrollBar = GetTemplateChild(ScrollBarName) as ScrollViewer;
            InnerPanel = GetTemplateChild(InnerPanelName) as StackPanel;
        }

        public void ScrollBottom()
        {
           ScrollBar.ChangeView(0, ScrollBar.ScrollableHeight, 0);
        }

        private void RefreshView()
        {
            if (timer == null)
            {
                initTimer();
            }
            timer.Stop();
            InnerPanel.Children.Clear();
            if (Items == null)
            {
                return;
            }
            BindListener();
            DateTime lastTime = DateTime.MinValue;
            var now = DateTime.Now;
            var exist = new List<int>();
            foreach (var item in Items)
            {
                if (exist.IndexOf(item.Id) >= 0)
                {
                    continue;
                }
                var time = Time.TimestampTo(item.CreatedAt);
                if (lastTime == DateTime.MinValue || Time.SecondDiffer(time, lastTime) > MaxTime)
                {
                    lastTime = time;
                    InnerPanel.Children.Add(createTime(Time.FormatAgo(time, now), time));
                }
                exist.Add(item.Id);
                InnerPanel.Children.Add(createMessage(item));
            }
            timer.Start();
        }

        private void initTimer()
        {
            if (timer != null)
            {
                return;
            }
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
            timer.Tick += new EventHandler<object>(async (sender, e) =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    var now = DateTime.Now;
                    foreach (var item in InnerPanel.Children)
                    {
                        if (item is MessageTimeItem)
                        {
                            var obj = item as MessageTimeItem;
                            obj.Text = Time.FormatAgo(obj.Source, now);
                        }
                    }
                });
            });
        }

        private FrameworkElement createTime(string value, DateTime data)
        {
            return new MessageTimeItem()
            {
                Text = value,
                Source = data,
                FontFamily = FontFamily,
            };
        }

        private FrameworkElement createMessage(MessageBase item)
        {
            var obj = new MessageListItem()
            {
                Source = item,
                FontFamily = FontFamily,
            };
            if (item.User != null && item.User.Id == Sender && Sender > 0)
            {
                obj.FlowDirection = FlowDirection.RightToLeft;
            }
            return obj;
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
