using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System.Threading.Tasks;
using ZoDream.LogTimer.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Member
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!ViewModel.Load())
            {
                return;
            }
            var imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("avatar");
            imageAnimation?.TryStart(avatarImg);
        }

        private void TipMenuItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var label = (sender as IconLine).Name;
            var pageType = label == "pwdBtn" ? typeof(PasswordPage)
                : label == "connectBtn" ? typeof(Account.ConnectPage)
                : label == "driverBtn" ? typeof(Account.DriverPage)
                : label == "cancelBtn" ? typeof(Account.CancelPage)
                : null;
            if (pageType != null)
            {
                Frame.Navigate(pageType);
            }
        }

        
    }
}
