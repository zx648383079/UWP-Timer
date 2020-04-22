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
    public sealed partial class LargeHeader : UserControl
    {
        public LargeHeader()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(LargeHeader), new PropertyMetadata(string.Empty));


        /// <summary>
        /// 是否显示提交按钮
        /// </summary>
        public bool CanSubmit
        {
            get { return (bool)GetValue(CanSubmitProperty); }
            set { SetValue(CanSubmitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasBtn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanSubmitProperty =
            DependencyProperty.Register("CanSubmit", typeof(bool), typeof(LargeHeader), new PropertyMetadata(true));

        private void Border_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Submited?.Invoke(this, e);
        }

        /// <summary>
        /// 提交按钮事件
        /// </summary>
        public event TappedEventHandler Submited;
    }
}
