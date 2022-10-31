using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    public sealed class CountDownButton : Control
    {
        public CountDownButton()
        {
            this.DefaultStyleKey = typeof(CountDownButton);
        }

        private int _time = 0;
        private DispatcherTimer _timer;

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);
            if (_time > 0)
            {
                e.Handled = true;
            }
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(CountDownButton), new PropertyMetadata("获取验证码"));



        public void Start(int time = 60)
        {
            _time = time;
            if (_timer != null)
            {
                _timer.Start();
                return;
            }
            _timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
            _timer.Tick += new EventHandler<object>((sender, e) =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    _time--;
                    if (_time < 1)
                    {
                        Label = "重新获取";
                        return;
                    }
                    Label = _time.ToString("00");
                });
            });
            _timer.Start();
        }
    }
}
