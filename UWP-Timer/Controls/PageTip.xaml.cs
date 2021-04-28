using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class PageTip : UserControl
    {
        public PageTip()
        {
            this.InitializeComponent();
        }



        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(PageTip), new PropertyMetadata("提示"));




        public string Tip
        {
            get { return (string)GetValue(TipProperty); }
            set { SetValue(TipProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Tip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TipProperty =
            DependencyProperty.Register("Tip", typeof(string), typeof(PageTip), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnLineChange)));



        private static void OnLineChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PageTip).RefreshLines();
        }

        public bool Open
        {
            get { return (bool)GetValue(OpenProperty); }
            set { SetValue(OpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Open.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenProperty =
            DependencyProperty.Register("Open", typeof(bool), typeof(PageTip), new PropertyMetadata(true, new PropertyChangedCallback(OnOpenChange)));

        private static void OnOpenChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PageTip).RefreshView();
        }

        public void RefreshLines()
        {
            contentBox.Children.Clear();
            var lines = Tip.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                contentBox.Children.Add(new TextBlock()
                {
                    Text = "· " + line
                });
            }
        }

        public void RefreshView()
        {
            if (Open)
            {
                toggleIcon.Text = "-";
                contentBox.Visibility = Visibility.Visible;
                borderBox.Width = Width;
                borderBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                return;
            }
            toggleIcon.Text = "+";
            contentBox.Visibility = Visibility.Collapsed;
            borderBox.Width = Width < 200 ? Width : 200;
            borderBox.HorizontalAlignment = HorizontalAlignment.Left;
        }

        private void toggleIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Open = !Open;
        }
    }
}
