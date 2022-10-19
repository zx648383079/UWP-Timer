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
    [TemplatePart(Name = "ActionBtn", Type = typeof(FrameworkElement))]
    public sealed class LargeHeader : Control
    {
        public LargeHeader()
        {
            this.DefaultStyleKey = typeof(LargeHeader);
            Loaded += LargeHeader_Loaded;
            PointerEntered += LargeHeader_PointerEntered;
            PointerExited += LargeHeader_PointerExited;
        }

        private void LargeHeader_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void LargeHeader_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PointerOver", true);
        }

        private void LargeHeader_Loaded(object sender, RoutedEventArgs e)
        {
            var actionBtn = GetTemplateChild("ActionBtn") as FrameworkElement;
            if (actionBtn != null)
            {
                actionBtn.Tapped += ActionBtn_Tapped;
                RefreshSubmit();
            }
        }

        private void ActionBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Submited?.Invoke(this, e);
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
            DependencyProperty.Register("CanSubmit", typeof(bool), typeof(LargeHeader), new PropertyMetadata(true, new PropertyChangedCallback(OnSubmitToggle)));

        private static void OnSubmitToggle(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LargeHeader).RefreshSubmit();
        }

        private void RefreshSubmit()
        {
            var actionBtn = GetTemplateChild("ActionBtn") as FrameworkElement;
            actionBtn.Visibility = CanSubmit ? Visibility.Visible : Visibility.Collapsed;
        }


        /// <summary>
        /// 提交按钮事件
        /// </summary>
        public event TappedEventHandler Submited;
    }
}
