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
    public sealed class IconMenuItem : ContentControl
    {
        public IconMenuItem()
        {
            this.DefaultStyleKey = typeof(IconMenuItem);
            PointerEntered += IconMenuItem_PointerEntered;
            PointerExited += IconMenuItem_PointerExited;

        }

        private void IconMenuItem_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void IconMenuItem_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PointerOver", true);
        }

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
            DependencyProperty.Register("Icon", typeof(string), typeof(IconMenuItem), 
                new PropertyMetadata(default(string), (s, d) =>
                {
                    (s as IconMenuItem).IconVisibility = string.IsNullOrWhiteSpace(d.NewValue as string) ? Visibility.Collapsed : Visibility.Visible;
                }));



        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(IconMenuItem), 
                new PropertyMetadata(default(string)));



        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(IconMenuItem), new PropertyMetadata(default(string)));



        public Visibility IconVisibility
        {
            get { return (Visibility)GetValue(IconVisibilityProperty); }
            set { SetValue(IconVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconVisibilityProperty =
            DependencyProperty.Register("IconVisibility", typeof(Visibility), typeof(IconMenuItem), new PropertyMetadata(Visibility.Visible));




        public Visibility RightIconVisibility
        {
            get { return (Visibility)GetValue(RightIconVisibilityProperty); }
            set { SetValue(RightIconVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RightIconVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightIconVisibilityProperty =
            DependencyProperty.Register("RightIconVisibility", typeof(Visibility), typeof(IconMenuItem), new PropertyMetadata(Visibility.Visible));



    }
}
