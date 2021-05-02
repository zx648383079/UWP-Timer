using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Timer.Models;
using UWP_Timer.Utils;
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
    public sealed partial class MessageContainer : UserControl
    {
        public MessageContainer()
        {
            this.InitializeComponent();
        }

        private DispatcherTimer timer;

        public int Sender
        {
            get { return (int)GetValue(SenderProperty); }
            set { SetValue(SenderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Sender.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SenderProperty =
            DependencyProperty.Register("Sender", typeof(int), typeof(MessageContainer), new PropertyMetadata(0));



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
            ContainerBox.Children.Clear();
            if (Items == null)
            {
                return;
            }
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
                    ContainerBox.Children.Add(createTime(Time.FormatAgo(time, now), time));
                }
                exist.Add(item.Id);
                ContainerBox.Children.Add(createMessage(item));
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
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            timer.Tick += new EventHandler<object>(async (sender, e) =>
            {
                await dispatcherQueue.EnqueueAsync(() =>
                {
                    var now = DateTime.Now;
                    foreach (var item in ContainerBox.Children)
                    {
                        if (item is MessageTime)
                        {
                            var obj = item as MessageTime;
                            obj.Text = Time.FormatAgo(obj.Source, now);
                        }
                    }
                });
            });
        }

        private FrameworkElement createTime(string value, DateTime data)
        {
            return new MessageTime()
            {
                Text = value,
                Source = data
            };
        }

        private FrameworkElement createMessage(MessageBase item)
        {
            var obj = new MessageItem()
            {
                Source = item
            };
            if (item.User != null && item.User.Id == Sender)
            {
                obj.FlowDirection = FlowDirection.RightToLeft;
            }
            return obj;
        }
    }
}
