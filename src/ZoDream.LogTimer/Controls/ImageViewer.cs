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
using ZoDream.LogTimer.Converters;
using ZoDream.LogTimer.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    [TemplatePart(Name = PreviewPanelName, Type = typeof(GridView))]
    [TemplatePart(Name = ThumbPanelName, Type = typeof(ListBox))]
    [TemplatePart(Name = ToggleBtnName, Type = typeof(FrameworkElement))]
    public sealed class ImageViewer : Control
    {
        const string PreviewPanelName = "PART_PreviewPanel";
        const string ThumbPanelName = "PART_ThumbPanel";
        const string ToggleBtnName = "PART_ToggleBtn";
        public ImageViewer()
        {
            this.DefaultStyleKey = typeof(ImageViewer);
        }

        private GridView PreviewPanel;
        private ListBox ThumbPanel;
        private FrameworkElement ToggleBtn;


        public IEnumerable<MicroAttachment> Items
        {
            get { return (IEnumerable<MicroAttachment>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable<MicroAttachment>), typeof(ImageViewer), new PropertyMetadata(null));




        public Visibility PreviewVisibily
        {
            get { return (Visibility)GetValue(PreviewVisibilyProperty); }
            set { SetValue(PreviewVisibilyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreviewVisibily.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreviewVisibilyProperty =
            DependencyProperty.Register("PreviewVisibily", typeof(Visibility), typeof(ImageViewer), new PropertyMetadata(Visibility.Visible));




        public Visibility LargeVisibily
        {
            get { return (Visibility)GetValue(LargeVisibilyProperty); }
            set { SetValue(LargeVisibilyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LargeVisibily.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LargeVisibilyProperty =
            DependencyProperty.Register("LargeVisibily", typeof(Visibility), typeof(ImageViewer), new PropertyMetadata(Visibility.Collapsed));


        public string LargeImage
        {
            get { return (string)GetValue(LargeImageProperty); }
            set { SetValue(LargeImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LargeImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LargeImageProperty =
            DependencyProperty.Register("LargeImage", typeof(string), typeof(ImageViewer), new PropertyMetadata(default(string)));



        public bool TogglePreview
        {
            get { return (bool)GetValue(TogglePreviewProperty); }
            set { SetValue(TogglePreviewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TogglePreview.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TogglePreviewProperty =
            DependencyProperty.Register("TogglePreview", typeof(bool), typeof(ImageViewer), new PropertyMetadata(true, (d, s) => {
                var sender = d as ImageViewer;
                var val = (bool)s.NewValue;
                sender.PreviewVisibily = val ? Visibility.Visible : Visibility.Collapsed;
                sender.LargeVisibily = val ? Visibility.Collapsed : Visibility.Visible;
            }));




        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PreviewPanel = GetTemplateChild(PreviewPanelName) as GridView;
            ThumbPanel = GetTemplateChild(ThumbPanelName) as ListBox;
            ToggleBtn = GetTemplateChild(ToggleBtnName) as Button;
            if (ToggleBtn != null) 
            {
                ToggleBtn.Tapped += ToggleBtn_Tapped;
            }
            if (PreviewPanel != null)
            {
                PreviewPanel.SelectionChanged += PreviewPanel_SelectionChanged;
            }
            if (ThumbPanel != null)
            {
                ThumbPanel.SelectionChanged += ThumbPanel_SelectionChanged;
            }
        }

        private void ThumbPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenLarge(ThumbPanel.SelectedIndex);
        }

        private void PreviewPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenLarge(PreviewPanel.SelectedIndex);
        }

        private void ToggleBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TogglePreview = true;
        }

        private void OpenLarge(int selectedIndex)
        {
            TogglePreview = false;
            ThumbPanel.SelectedIndex = selectedIndex;
            LargeImage = Items.ToList()[selectedIndex].File;
        }
    }
}
