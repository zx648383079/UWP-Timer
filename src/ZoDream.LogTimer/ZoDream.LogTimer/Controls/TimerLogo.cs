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
using ZoDream.LogTimer.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    public sealed class TimerLogo : Control
    {
        public TimerLogo()
        {
            this.DefaultStyleKey = typeof(TimerLogo);
        }
        public LogoStatus Status
        {
            get { return (LogoStatus)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(LogoStatus), typeof(TimerLogo), new PropertyMetadata(LogoStatus.NONE, OnStatusChange));

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
            VisualStateManager.GoToState(d as TimerLogo, val, true);
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
