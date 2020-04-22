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
    public sealed partial class IconMenuItem : UserControl
    {
        public IconMenuItem()
        {
            this.InitializeComponent();
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
            DependencyProperty.Register("Label", typeof(string), typeof(IconMenuItem), new PropertyMetadata(null));


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
            DependencyProperty.Register("Icon", typeof(string), typeof(IconMenuItem), new PropertyMetadata(null));


        /// <summary>
        /// 右边的说明
        /// </summary>
        public string Tip
        {
            get { return (string)GetValue(TipProperty); }
            set { SetValue(TipProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Tip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TipProperty =
            DependencyProperty.Register("Tip", typeof(string), typeof(IconMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// 是否有底边
        /// </summary>
        public bool HasBottomBorder
        {
            get { return (bool)GetValue(HasBottomBorderProperty); }
            set { SetValue(HasBottomBorderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasBottomBorder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasBottomBorderProperty =
            DependencyProperty.Register("HasBottomBorder", typeof(bool), typeof(IconMenuItem), new PropertyMetadata(true));
    }
}
