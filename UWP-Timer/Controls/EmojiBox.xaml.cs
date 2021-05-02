using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWP_Timer.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
    public sealed partial class EmojiBox : UserControl
    {
        public EmojiBox()
        {
            this.InitializeComponent();
            Loaded += EmojiBox_Loaded;
        }


        DispatcherQueue dispatcherQueue;
        private IList<EmojiCategory> Source = null;

        private void EmojiBox_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (Source != null)
            {
                return;
            }
            Source = await App.Repository.Site.GetEmojiAsync();
            if (Source == null)
            {
                return;
            }
            await dispatcherQueue.EnqueueAsync(() =>
            {
                CatBox.Items.Clear();
                foreach (var item in Source)
                {
                    CatBox.Items.Add(item.Name);
                }
                SelectedCategory(0);
            });
        }

        private void SelectedCategory(int v)
        {
            if (Source == null)
            {
                return;
            }
            CatBox.SelectedIndex = v;
        }

        private void CatBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemBox.Items.Clear();
            ItemBox.SelectedIndex = -1;
            if (Source == null)
            {
                return;
            }
            var cat = Source[CatBox.SelectedIndex];
            if (cat == null)
            {
                return;
            }
            foreach (var item in cat.Items)
            {
                if (item.Type > 0)
                {
                    ItemBox.Items.Add(new TextBlock()
                    {
                        Text = item.Content,
                        Width = 100,
                        Height = 40,
                        TextAlignment = TextAlignment.Center
                    });
                    continue;
                }
                ItemBox.Items.Add(new Image()
                {
                    Source = Converters.ConverterHelper.ToImg(item.Content),
                    Width = 60,
                    Height = 60
                });
            }
        }

        private void ItemBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Source == null || ItemBox.SelectedIndex < 0)
            {
                return;
            }
            var cat = Source[CatBox.SelectedIndex];
            if (cat == null)
            {
                return;
            }
            SelectionChanged?.Invoke(this, new EmojiTappedArgs(cat.Items[ItemBox.SelectedIndex]));
        }

        public event TypedEventHandler<EmojiBox, EmojiTappedArgs> SelectionChanged;
    }

    public class EmojiTappedArgs
    {
        public readonly Emoji Emoji;

        public EmojiTappedArgs()
        {

        }

        public EmojiTappedArgs(Emoji emoji)
        {
            Emoji = emoji;
        }
    }
}
