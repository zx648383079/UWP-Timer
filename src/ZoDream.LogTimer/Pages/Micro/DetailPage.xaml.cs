using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Utils;
using ZoDream.LogTimer.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Micro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        public DetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Load((MicroItem)e.Parameter);
            ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("micro");
            imageAnimation?.TryStart(MicroView);
        }

        
        private void EmojiBox_SelectionChanged(Controls.EmojiBox sender, Controls.EmojiTappedArgs args)
        {
            CommentTb.SelectedText = args.Emoji.Type > 0 ? args.Emoji.Content : $"[{args.Emoji.Name}]";
            emojiFlyout.Hide();
        }


        private void CommentListBox_ActionTapped(Controls.CommentListItem sender, ActionArgs<CommentBase> args)
        {
            if (args.Action == ActionType.REPLY)
            {
                ViewModel.ReplyCommand.Execute(args.Data);
            }
        }
    }
}
