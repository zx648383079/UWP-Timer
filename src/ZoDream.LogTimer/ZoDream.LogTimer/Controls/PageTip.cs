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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    [TemplatePart(Name = InnerPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = ToggleIconName, Type = typeof(TextBlock))]
    public sealed class PageTip : ContentControl
    {
        const string ToggleIconName = "PART_ToggleIcon";
        const string InnerPanelName = "PART_InnerPanel";
        public PageTip()
        {
            this.DefaultStyleKey = typeof(PageTip);
            Loaded += PageTip_Loaded;
        }

        private void PageTip_Loaded(object sender, RoutedEventArgs e)
        {
            var toggleIcon = GetTemplateChild(ToggleIconName) as TextBlock;
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
            var contentBox = GetTemplateChild(InnerPanelName) as StackPanel;
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
