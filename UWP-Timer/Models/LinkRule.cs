using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
{
    public class ExtraRule
    {
        public string S { get; set; }

        public int U { get; set; }

        public string I { get; set; }
        public string L { get; set; }
        public string F { get; set; }

        public BlockType Type { get; set; }

        public BlockItem ToBlock()
        {
            if (I != null)
            {
                return new BlockItem(BlockType.IMAGE, S, I);
            }
            if (U > 0)
            {
                return new BlockItem(BlockType.USER, S, U);
            }
            if (L != null)
            {
                return new BlockItem(BlockType.LINK, S, L);
            }
            if (F != null)
            {
                return new BlockItem(BlockType.FILE, S, F);
            }
            return new BlockItem(Type, S);
        }
    }

    public enum BlockType
    {
        TEXT,
        USER,
        IMAGE,
        LINK,
        FILE,
        LINE = 99,
    }

    public class BlockItem
    {
        public BlockType Type { get; set; }

        public string Content { get; set; }

        public object Value { get; set; }


        public BlockItem Clone()
        {
            return new BlockItem(Type, Content, Value);
        }

        public BlockItem()
        {

        }

        public BlockItem(string content)
        {
            Type = BlockType.TEXT;
            Content = content;
        }

        public BlockItem(BlockType blockType, string content)
        {
            Type = blockType;
            Content = content;
        }

        public BlockItem(BlockType blockType, string content, object value)
        {
            Type = blockType;
            Content = content;
            Value = value;
        }
    }
}
