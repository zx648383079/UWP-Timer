using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Timer.Converters;
using UWP_Timer.Models;
using UWP_Timer.Utils;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace UWP_Timer.Controls
{
    public sealed partial class RuleBlock : UserControl
    {
        public RuleBlock()
        {
            this.InitializeComponent();
        }

        public event TypedEventHandler<RuleBlock, RuleTappedArgs> RuleTapped;

        public string Block
        {
            get { return (string)GetValue(BlockProperty); }
            set { SetValue(BlockProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlockProperty =
            DependencyProperty.Register("Block", typeof(string), typeof(RuleBlock), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnContentChange)));

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
            viewer.Blocks.Clear();
            if (string.IsNullOrWhiteSpace(Block))
            {
                return;
            }
            var items = LinkRule.Render(Block, Rules, true);
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
                    };
                    paragraph.Inlines.Add(run);
                    continue;
                }
                if (item.Type == BlockType.LINK)
                {
                    var link = new Hyperlink()
                    {
                        NavigateUri = new Uri(item.Value as string),
                    };
                    link.Click += (Hyperlink sender, HyperlinkClickEventArgs e) =>
                    {
                        RuleTapped.Invoke(this, new RuleTappedArgs(item));
                    };
                    link.Inlines.Add(new Run()
                    {
                        Text = item.Content
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
                    container.Child = img;
                    paragraph.Inlines.Add(container);
                }
            }
            viewer.Blocks.Add(paragraph);
        }
    }

    public class RuleTappedArgs
    {
        public BlockItem Block { get; set; }

        public RuleTappedArgs()
        {

        }

        public RuleTappedArgs(BlockItem block)
        {
            Block = block;
        }
    }
}
