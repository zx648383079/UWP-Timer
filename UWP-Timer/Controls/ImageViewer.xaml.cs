using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Timer.Converters;
using UWP_Timer.Models;
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
    public sealed partial class ImageViewer : UserControl
    {
        public ImageViewer()
        {
            this.InitializeComponent();
        }

        public IEnumerable<MicroAttachment> Items
        {
            get { return (IEnumerable<MicroAttachment>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable<MicroAttachment>), typeof(ImageViewer), new PropertyMetadata(null, new PropertyChangedCallback(OnItemsChange)));

        private static void OnItemsChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ImageViewer).RefreshView();
        }

        private void RefreshView()
        {
            var items = Items.ToList();
            if (items.Count < 1)
            {
                return;
            }
            PreviewBox.ItemsSource = items;
            LargeThumb.ItemsSource = items;
        }



        private void PreviewBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenLarge(PreviewBox.SelectedIndex);
        }

        private void OpenLarge(int selectedIndex)
        {
            PreviewBox.Visibility = Visibility.Collapsed;
            LargeBox.Visibility = Visibility.Visible;
            LargeImage.Source = ConverterHelper.ToImg(Items.ToList()[selectedIndex].File);
        }

        private void ToggleBtn_Click(object sender, RoutedEventArgs e)
        {
            LargeBox.Visibility = Visibility.Collapsed;
            PreviewBox.Visibility = Visibility.Visible;
        }

        private void LargeThumb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenLarge(LargeThumb.SelectedIndex);
        }
    }
}
