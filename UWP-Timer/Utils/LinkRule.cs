using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;

namespace UWP_Timer.Utils
{
    public static class LinkRule
    {
        public static List<BlockItem> Render(string content, IEnumerable<ExtraRule> rules, bool newLine = false)
        {
            if (content == null || string.IsNullOrEmpty(content))
            {
                content = "";
            }
            var items = new List<BlockItem>
            {
                new BlockItem(content)
            };
            if (newLine)
            {
                items = SplitArr(items, new ExtraRule() { Type = BlockType.LINE, S = "\n" });
            }
            if (rules != null)
            {
                foreach (var rule in rules)
                {
                    items = SplitArr(items, rule);
                }
            }
            return items;
        }

        private static List<BlockItem> SplitArr(List<BlockItem> items, ExtraRule rule)
        {
            var data = new List<BlockItem>();
            var block = rule.ToBlock();
            foreach (var item in items)
            {
                if (item.Type != BlockType.TEXT)
                {
                    data.Add(item);
                    continue;
                }
                var lines = item.Content.Split(rule.S);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i > 0)
                    {
                        data.Add(block.Clone());
                    }
                    var line = lines[i];
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    data.Add(new BlockItem(line));
                }
            }
            return data;
        }
    }
}
