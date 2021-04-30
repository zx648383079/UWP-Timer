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
    public sealed class IconLine : Button
    {
        public IconLine()
        {
            this.DefaultStyleKey = typeof(IconLine);
            // PointerEntered += IconLine_PointerEntered;
        }

        private void IconLine_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PointerOver", true);
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(IconLine), new PropertyMetadata(null));


        /// <summary>
        /// 字体图标
        /// </summary>
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(IconLine), new PropertyMetadata(null, new PropertyChangedCallback(OnIconChange)));

        private static void OnIconChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as IconLine;
            control.IconVisibility = !string.IsNullOrWhiteSpace(control.Icon) ? Visibility.Visible : Visibility.Collapsed;
        }

        public Visibility BottomBorderVisibility
        {
            get { return (Visibility)GetValue(BottomBorderVisibilityProperty); }
            set { SetValue(BottomBorderVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BottomBorderVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BottomBorderVisibilityProperty =
            DependencyProperty.Register("BottomBorderVisibility", typeof(Visibility), typeof(IconLine), new PropertyMetadata(Visibility.Visible));



        public Visibility IconVisibility
        {
            get { return (Visibility)GetValue(IconVisibilityProperty); }
            set { SetValue(IconVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconVisibilityProperty =
            DependencyProperty.Register("IconVisibility", typeof(Visibility), typeof(IconLine), new PropertyMetadata(Visibility.Collapsed));




        public double BottomBorderHeight
        {
            get { return (double)GetValue(BottomBorderHeightProperty); }
            set { SetValue(BottomBorderHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BottomBorderHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BottomBorderHeightProperty =
            DependencyProperty.Register("BottomBorderHeight", typeof(double), typeof(IconLine), new PropertyMetadata(1, new PropertyChangedCallback(OnBottomBorderChange)));

        private static void OnBottomBorderChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as IconLine;
            control.BottomBorderVisibility = control.BottomBorderHeight > 0 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
