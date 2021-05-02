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
    public sealed partial class Logo : UserControl
    {
        public Logo()
        {
            this.InitializeComponent();
        }



        public LogoStatus Status
        {
            get { return (LogoStatus)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(LogoStatus), typeof(Logo), new PropertyMetadata(LogoStatus.NONE, OnStatusChange));

        private static void OnStatusChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var val = "CommonState";
            switch (e.NewValue)
            {
                case LogoStatus.INIT:
                    val = "InitState";
                    break;
                case LogoStatus.HALF:
                    val = "HalfState";
                    break;
                case LogoStatus.END:
                    val = "EndState";
                    break;
                default:
                    break;
            }
            VisualStateManager.GoToState(d as Logo, val, true);
        }
    }

    public enum LogoStatus
    {
        NONE,
        INIT,
        HALF,
        END
    }
}
