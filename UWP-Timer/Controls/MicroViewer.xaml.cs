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
    public sealed partial class MicroViewer : UserControl
    {
        public MicroViewer()
        {
            this.InitializeComponent();
        }

        public event TypedEventHandler<MicroViewer, object> CommentTapped;
        public event TypedEventHandler<MicroViewer, object> LikeTapped;
        public event TypedEventHandler<MicroViewer, object> CollectTapped;
        public event TypedEventHandler<MicroViewer, object> ForwardTapped;
        public event TypedEventHandler<MicroViewer, RuleTappedArgs> RuleTapped;

        public MicroItem Source
        {
            get { return (MicroItem)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(MicroItem), typeof(MicroViewer), new PropertyMetadata(null, OnSourceChange));

        private static void OnSourceChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MicroViewer).RefreshView();
        }

        private void RefreshView()
        {
            if (Source == null)
            {
                return;
            }
            MicroAgo.Text = ConverterHelper.Ago(Source.CreatedAt);
            if (Source.User != null)
            {
                MicroAvatar.ProfilePicture = ConverterHelper.ToImg(Source.User.Avatar);
                MicroName.Text = MicroAvatar.DisplayName = Source.User.Name;
            }
            MicroContent.Content = Source.Content;
            MicroContent.Rules = Source.ExtraRule;
            MicroFile.Items = Source.Attachment;
        }

        private void CollectBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CollectTapped?.Invoke(this, null);
        }

        private void ForwardBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ForwardTapped?.Invoke(this, null);
        }

        private void CommentBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CommentTapped?.Invoke(this, null);
        }

        private void LikeBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            LikeTapped?.Invoke(this, null);
        }

        private void MicroContent_RuleTapped(RuleBlock sender, RuleTappedArgs args)
        {
            RuleTapped?.Invoke(this, args);
        }
    }
}
