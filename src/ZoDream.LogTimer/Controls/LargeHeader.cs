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
    [TemplatePart(Name = ActionBtnName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ActionBtnBodyName, Type = typeof(FrameworkElement))]
    public sealed class LargeHeader : ContentControl
    {
        const string ActionBtnName = "PART_ActionBtn";
        const string ActionBtnBodyName = "PART_ActionContent";

        public LargeHeader()
        {
            this.DefaultStyleKey = typeof(LargeHeader);
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
            if (ActionBtn is null)
            {
                return;
            }
            ActionBtn.Visibility = CanSubmit ? Visibility.Visible : Visibility.Collapsed;
        }


        /// <summary>
        /// 提交按钮事件
        /// </summary>
        public event TappedEventHandler Submitted;
        private FrameworkElement ActionBtn;
        private FrameworkElement ActionBodyBtn;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ActionBtn = GetTemplateChild(ActionBtnName) as FrameworkElement;
            ActionBodyBtn = GetTemplateChild(ActionBtnBodyName) as FrameworkElement;
            if (ActionBtn != null)
            {
                ActionBtn.Tapped += ActionBtn_Tapped;
            }
            RefreshSubmit();
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            VisualStateManager.GoToState(this, "PointerOver", true);
            if (ActionBodyBtn is not null)
            {
                ActionBodyBtn.Translation += new System.Numerics.Vector3(0, -10, 20);
            }
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            VisualStateManager.GoToState(this, "Normal", true);
            if (ActionBodyBtn is not null)
            {
                ActionBodyBtn.Translation -= new System.Numerics.Vector3(0, -10, 20);
            }
        }

        private void ActionBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Submitted?.Invoke(this, e);
        }
    }
}
