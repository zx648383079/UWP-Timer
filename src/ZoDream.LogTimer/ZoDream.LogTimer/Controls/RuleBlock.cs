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
using Windows.Foundation;
using ZoDream.LogTimer.Converters;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    [TemplatePart(Name = "Viewer", Type = typeof(RichTextBlock))]
    public sealed class RuleBlock : Control
    {
        public RuleBlock()
        {
            this.DefaultStyleKey = typeof(RuleBlock);
            Loaded += RuleBlock_Loaded;
        }

        private void RuleBlock_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Content))
            {
                RefreshRule();
            }
        }

        public event TypedEventHandler<RuleBlock, RuleTappedArgs> RuleTapped;

        public string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(string), typeof(RuleBlock), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnContentChange)));

        private static void OnContentChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as RuleBlock).RefreshRule();
        }

        public IEnumerable<ExtraRule> Rules
        {
            get { return (IEnumerable<ExtraRule>)GetValue(RulesProperty); }
            set { SetValue(RulesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rules.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RulesProperty =
            DependencyProperty.Register("Rules", typeof(IEnumerable<ExtraRule>), typeof(RuleBlock), new PropertyMetadata(null, new PropertyChangedCallback(OnContentChange)));


        public void RefreshRule()
        {
            var viewer = GetTemplateChild("Viewer") as RichTextBlock;
            if (viewer == null)
            {
                return;
            }
            viewer.Blocks.Clear();
            if (string.IsNullOrWhiteSpace(Content))
            {
                return;
            }
            var items = LinkRule.Render(Content, Rules, true);
            var paragraph = new Paragraph();
            foreach (var item in items)
            {
                if (item.Type == BlockType.LINE)
                {
                    paragraph.Inlines.Add(new LineBreak());
                    continue;
                }
                if (item.Type == BlockType.TEXT)
                {
                    var run = new Run
                    {
                        Text = item.Content,
                        FontSize = FontSize,
                    };
                    paragraph.Inlines.Add(run);
                    continue;
                }
                if (item.Type == BlockType.LINK || item.Type == BlockType.USER)
                {
                    var link = new Hyperlink()
                    {
                        // NavigateUri = new Uri(item.Value as string),
                    };
                    link.Click += (Hyperlink sender, HyperlinkClickEventArgs e) =>
                    {
                        RuleTapped?.Invoke(this, new RuleTappedArgs(item));
                    };
                    link.Inlines.Add(new Run()
                    {
                        Text = item.Content,
                        FontSize = FontSize,
                    });
                    paragraph.Inlines.Add(link);
                    continue;
                }
                if (item.Type == BlockType.IMAGE)
                {
                    var container = new InlineUIContainer();
                    var img = new Image
                    {
                        Source = ConverterHelper.ToImg(item.Value as string)
                    };
                    img.Width = 1.5 * FontSize;
                    container.Child = img;
                    paragraph.Inlines.Add(container);
                }
            }
            viewer.TextWrapping = TextWrapping.Wrap;
            viewer.LineHeight = 2 * FontSize;
            viewer.Blocks.Add(paragraph);
        }
    }

    public class RuleTappedArgs
    {
        public readonly BlockItem Block;

        public RuleTappedArgs()
        {

        }

        public RuleTappedArgs(BlockItem block)
        {
            Block = block;
        }
    }
}
