using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Runtime.InteropServices;
using WinRT.Interop;
//using static Vanara.PInvoke.Shell32;
//using Vanara.PInvoke;
using ZoDream.LogTimer.Services;
using ZoDream.LogTimer.Converters;
using Windows.UI.WindowManagement;

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
            // Berth(this, 200);
            CustomWindow();
            _task.TaskChanged += Task_TaskChanged;
            _task.PausedChanged += Task_PausedChanged;
            _task.TimeUpdated += Task_TimeUpdated;


        }

        private readonly TaskService _task = App.GetService<TaskService>();

        public bool IsPaused {
            get => FrontPanel.Visibility == Visibility.Visible;
            set {
                FrontPanel.Visibility = !value ? Visibility.Visible : Visibility.Collapsed;
                BackPanel.Visibility = !value ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private void CustomWindow()
        {
            //var _baseWindowHandle = WindowNative.GetWindowHandle(this);
            //var windowId = Win32Interop.GetWindowIdFromWindow(_baseWindowHandle);
            //var _appWindow = AppWindow.GetFromWindowId(windowId);
            AppWindow.MoveAndResize(new Windows.Graphics.RectInt32(200, 200, 300, 120));
            // AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            AppWindow.SetPresenter(OverlappedPresenter.CreateForContextMenu());
        }
        /*
        /// <summary>
        /// 将窗口停靠到边缘
        /// </summary>
        static void Berth(Window window, int width = 200, int height = 80)
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
             data.rc.top = 120;
             data.rc.bottom = height + data.rc.top;//DisplayArea.Primary.OuterBounds.Height;
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
 
             //使用 win32Api 的SetWindowLong 函数将任务栏里面的应用图标去掉
             var style = User32.GetWindowLong(hwnd, User32.WindowLongFlags.GWL_EXSTYLE);
             style |= (int)User32.WindowStylesEx.WS_EX_TOOLWINDOW;
             User32.SetWindowLong(hwnd, User32.WindowLongFlags.GWL_EXSTYLE, style);
             //使用 win32Api MoveWindow 函数 更改窗口的大小和位置
             User32.MoveWindow(hwnd, data.rc.Left, data.rc.top, width, data.rc.Height, true);
         }

        /// <summary>
        /// 从边缘中取消停靠窗口
        /// </summary>
        static void Detach(Window window)
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
             var op = OverlappedPresenter.Create();
             var wid = Win32Interop.GetWindowIdFromWindow(hwnd);
             var aw = AppWindow.GetFromWindowId(wid);
             aw.SetPresenter(op);
             //在任务栏上显示图标
             var style = (int)User32.WindowStyles.WS_VISIBLE;
             User32.SetWindowLong(hwnd, User32.WindowLongFlags.GWL_EXSTYLE, style);
             //设置窗口大小和位置
             User32.MoveWindow(hwnd, 20, 200, 400, 400, true);
         }
        */
        private void Window_Closed(object sender, WindowEventArgs args)
        {
            // Detach(this);
            // App.Store.MiniTimer = null;
            _task.TaskChanged -= Task_TaskChanged;
            _task.PausedChanged -= Task_PausedChanged;
            _task.TimeUpdated -= Task_TimeUpdated;
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            _ = _task.PlayAsync();
        }

        private void Task_TimeUpdated()
        {
            DispatcherQueue.TryEnqueue(() => {
                //Duration = _task.Duration;
                //Progress = _task.Current;
                if (_task.Duration > 0)
                {
                    ProgressTb.Text = ConverterHelper.FormatHour(_task.Duration - _task.Current);
                    ProgressBar.Value = _task.Current * 100 / _task.Duration;
                } else
                {
                    ProgressTb.Text = ConverterHelper.FormatHour(_task.Current);
                    ProgressBar.IsIndeterminate = true;
                }
            });
        }

        private void Task_PausedChanged()
        {
            DispatcherQueue.TryEnqueue(() => {
                IsPaused = _task.Paused;
            });
        }

        private void Task_TaskChanged()
        {
            DispatcherQueue.TryEnqueue(() => {
                TimeTb.Text = _task.Duration.ToString();
                if (_task.Today is null)
                {
                    return;
                }
                NameTb.Text = _task.Today.Task.Name;
                DescTb.Text = _task.Today.Task.Description;
                DescTb.Visibility = string.IsNullOrWhiteSpace(_task.Today.Task.Description) ?
                        Visibility.Collapsed : Visibility.Visible;
            });
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            IsPaused = _task.Paused;
            TimeTb.Text = _task.Duration.ToString();
            if (_task.Today is null)
            {
                return;
            }
            NameTb.Text = _task.Today.Task.Name;
            DescTb.Text = _task.Today.Task.Description;
            DescTb.Visibility = string.IsNullOrWhiteSpace(_task.Today.Task.Description) ?
                    Visibility.Collapsed : Visibility.Visible;
        }
    }
}
