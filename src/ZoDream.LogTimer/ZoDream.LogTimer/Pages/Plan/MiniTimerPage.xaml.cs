using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PInvoke;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;
using static Vanara.PInvoke.Shell32;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Plan
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MiniTimerPage : Window
    {
        public MiniTimerPage()
        {
            this.InitializeComponent();
            Berth(this, 200);
        }

        /// <summary>
         /// 将窗口停靠到边缘
         /// </summary>
         void Berth(Window window, int width = 200, int height = 80)
         {
             //获取窗口句柄
             var hwnd = WindowNative.GetWindowHandle(window);
            //创建应用栏信息对象，并设置其大小和位置
            var data = new APPBARDATA
            {
                hWnd = hwnd,
                uEdge = ABE.ABE_RIGHT//设置方向
            };
             data.cbSize = (uint) Marshal.SizeOf(data);
             data.rc.Top = 120;
             data.rc.bottom = height;//DisplayArea.Primary.OuterBounds.Height;
             data.rc.left = DisplayArea.Primary.OuterBounds.Width - width;
             data.rc.right = DisplayArea.Primary.OuterBounds.Width;
             //调用 win32Api 设定指定位置为“AppBar”
             SHAppBarMessage(ABM.ABM_NEW, ref data);
             SHAppBarMessage(ABM.ABM_SETPOS, ref data);
 
             //使用 AppWindow 类将窗口设置为菜单模式（将标题栏去掉，并且设置为不可更改大小）;
             var wid = Win32Interop.GetWindowIdFromWindow(hwnd);
             var op = OverlappedPresenter.CreateForContextMenu();
             var appWindow = AppWindow.GetFromWindowId(wid);
             appWindow.SetPresenter(op);//将窗口设置为菜单模式
 
             //使用win32Api 的SetWindowLong 函数将任务栏里面的应用图标去掉
             var style = (User32.SetWindowLongFlags)User32.GetWindowLong(hwnd, User32.WindowLongIndexFlags.GWL_EXSTYLE);
             style |= User32.SetWindowLongFlags.WS_EX_TOOLWINDOW;
             User32.SetWindowLong(hwnd, User32.WindowLongIndexFlags.GWL_EXSTYLE, style);
             //使用 win32Api MoveWindow 函数 更改窗口的大小和位置
             User32.MoveWindow(hwnd, data.rc.Left, data.rc.top, width, data.rc.Height, true);
         }
 
         /// <summary>
         /// 从边缘中取消停靠窗口
         /// </summary>
         void Detach(Window window)
         {
             //获取窗口句柄
             var hwnd = WindowNative.GetWindowHandle(window);
            var data = new APPBARDATA
            {
                hWnd = hwnd
            };
            data.cbSize = (uint) Marshal.SizeOf(data);
             var d = SHAppBarMessage(ABM.ABM_REMOVE, ref data);
         
             //将窗口的模式设置为普通的模式，将标题栏显示出来
             OverlappedPresenter op = OverlappedPresenter.Create();
             var wid = Win32Interop.GetWindowIdFromWindow(hwnd);
             var aw = AppWindow.GetFromWindowId(wid);
             aw.SetPresenter(op);
             //在任务栏上显示图标
             var style = User32.SetWindowLongFlags.WS_VISIBLE;
             User32.SetWindowLong(hwnd, User32.WindowLongIndexFlags.GWL_EXSTYLE, style);
             //设置窗口大小和位置
             User32.MoveWindow(hwnd, 20, 200, 400, 400, true);
         }

        private void Window_Closed(object sender, WindowEventArgs args)
        {
            Detach(this);
        }
    }
}
