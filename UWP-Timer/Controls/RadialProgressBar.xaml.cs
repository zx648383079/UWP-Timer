using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class RadialProgressBar : UserControl
    {
        private DateTime _startTime;
        private DispatcherTimer _timer;

        public RadialProgressBar()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public int Max
        {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(int), typeof(RadialProgressBar), new PropertyMetadata(0));

        /// <summary>
        /// 当前进度
        /// </summary>
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(RadialProgressBar), new PropertyMetadata(0, valueChangedCallback));


        /// <summary>
        /// 圆的进度
        /// </summary>
        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Progress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress", typeof(double), typeof(RadialProgressBar), new PropertyMetadata(0.0));



        private static void valueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bar = d as RadialProgressBar;
            if (bar.Max > 0)
            {
                bar.Progress = (double)bar.Value * 100 / (bar.Max * 60);
            }
        }
        /// <summary>
        /// 圆边底色
        /// </summary>
        public Brush Outline
        {
            get { return (Brush)GetValue(OutlineProperty); }
            set { SetValue(OutlineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Outline.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OutlineProperty =
            DependencyProperty.Register("Outline", typeof(Brush), typeof(RadialProgressBar), new PropertyMetadata(new SolidColorBrush(Windows.UI.Colors.Gray)));

        /// <summary>
        /// 圆边已完成颜色
        /// </summary>
        public Brush Inline
        {
            get { return (Brush)GetValue(InlineProperty); }
            set { SetValue(InlineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Inline.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InlineProperty =
            DependencyProperty.Register("Inline", typeof(Brush), typeof(RadialProgressBar), new PropertyMetadata(new SolidColorBrush(Windows.UI.Colors.Black)));

        /// <summary>
        /// 进度事件
        /// </summary>
        public event RangeBaseValueChangedEventHandler ValueChanged;
        /// <summary>
        /// 进度完成最大值事件
        /// </summary>
        public event RangeBaseValueChangedEventHandler TimeEnd;
        /// <summary>
        /// 开始计时
        /// </summary>
        public void Start()
        {
            _startTime = DateTime.Now.AddSeconds(- Value);
            if (_timer != null)
            {
                _timer.Start();
                return;
            }
            _timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
            _timer.Tick += new EventHandler<object>(async (sender, e) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    var diff = (DateTime.Now - _startTime).TotalSeconds;
                    if (Max > 0 && diff >= Max * 60)
                    {
                        Stop();
                        return;
                    }
                    Value = Convert.ToInt32(diff);
                    ValueChanged?.Invoke(this, null);
                });
            });
            _timer.Start();
        }
        /// <summary>
        /// 停止计时
        /// </summary>
        public void Stop()
        {
            if (_timer == null)
            {
                return;
            }
            _timer.Stop();
            TimeEnd?.Invoke(this, null);
        }

    }
}
