using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace UWP_Timer.Controls
{
    [TemplatePart(Name = "ContentBox", Type = typeof(StackPanel))]
    [TemplatePart(Name = "ToggleIcon", Type = typeof(TextBlock))]
    public sealed class PageTip : ContentControl
    {
        public PageTip()
        {
            this.DefaultStyleKey = typeof(PageTip);
            Loaded += PageTip_Loaded;
        }

        private void PageTip_Loaded(object sender, RoutedEventArgs e)
        {
            var toggleIcon = GetTemplateChild("ToggleIcon") as TextBlock;
            if (toggleIcon != null)
            {
                toggleIcon.Tapped += ToggleIcon_Tapped;
            }
            RefreshLines();
        }

        private void ToggleIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Open = !Open;
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
            if (string.IsNullOrEmpty(Tip))
            {
                return;
            }
            var contentBox = GetTemplateChild("ContentBox") as StackPanel;
            if (contentBox == null)
            {
                return;
            }
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
            VisualStateManager.GoToState(this, Open ? "Normal" : "Min", true);
        }
    }
}
