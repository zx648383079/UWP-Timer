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
using ZoDream.LogTimer.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    public sealed class EmojiBox : Control
    {
        public EmojiBox()
        {
            this.DefaultStyleKey = typeof(EmojiBox);
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
