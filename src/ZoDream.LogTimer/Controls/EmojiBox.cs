using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    [TemplatePart(Name = ItemPanelName, Type = typeof(GridView))]
    [TemplatePart(Name = ItemHeaderName, Type = typeof(ListBox))]
    public sealed class EmojiBox : Control
    {
        const string ItemPanelName = "PART_ItemPanel";
        const string ItemHeaderName = "PART_ItemHeader";
        public EmojiBox()
        {
            this.DefaultStyleKey = typeof(EmojiBox);
            Loaded += EmojiBox_Loaded;
        }

        private void EmojiBox_Loaded(object sender, RoutedEventArgs e)
        {
            _ = LoadDataAsync();
        }

        public event TypedEventHandler<EmojiBox, EmojiTappedArgs> SelectionChanged;
        private GridView ItemPanel;
        private ListBox ItemHeader;
        private IList<EmojiCategory> Source = null;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ItemPanel = GetTemplateChild(ItemPanelName) as GridView;
            ItemHeader = GetTemplateChild(ItemHeaderName) as ListBox;
            if (ItemHeader != null)
            {
                ItemHeader.SelectionChanged += ItemHeader_SelectionChanged;
            }
            if (ItemPanel != null)
            {
                ItemPanel.SelectionChanged += ItemPanel_SelectionChanged;
            }
        }

        private async Task LoadDataAsync()
        {
            if (Source != null)
            {
                return;
            }
            Source = await App.GetService<RestSiteRepository>().GetEmojiAsync();
            if (Source == null)
            {
                return;
            }
            DispatcherQueue.TryEnqueue(() =>
            {
                ItemHeader.Items.Clear();
                foreach (var item in Source)
                {
                    ItemHeader.Items.Add(item.Name);
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
            ItemHeader.SelectedIndex = v;
        }

        private void ItemPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Source == null || ItemPanel.SelectedIndex < 0)
            {
                return;
            }
            var cat = Source[ItemHeader.SelectedIndex];
            if (cat == null)
            {
                return;
            }
            SelectionChanged?.Invoke(this, new EmojiTappedArgs(cat.Items[ItemPanel.SelectedIndex]));
        }

        private void ItemHeader_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemPanel.Items.Clear();
            ItemPanel.SelectedIndex = -1;
            if (Source == null)
            {
                return;
            }
            var cat = Source[ItemHeader.SelectedIndex];
            if (cat == null)
            {
                return;
            }
            foreach (var item in cat.Items)
            {
                if (item.Type > 0)
                {
                    ItemPanel.Items.Add(new TextBlock()
                    {
                        Text = item.Content,
                        Width = 100,
                        Height = 40,
                        TextAlignment = TextAlignment.Center
                    });
                    continue;
                }
                ItemPanel.Items.Add(new Image()
                {
                    Source = Converters.ConverterHelper.ToImg(item.Content),
                    Width = 60,
                    Height = 60
                });
            }
        }
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
