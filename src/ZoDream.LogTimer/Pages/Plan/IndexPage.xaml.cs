using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ZoDream.LogTimer.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Plan
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IndexPage : Page
    {
        public IndexPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            addBtn.Visibility = ViewModel.IsAuthenticated ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsAuthenticated)
            {
                Frame.Navigate(typeof(Auth.LoginPage));
                return;
            }
            var label = (sender as AppBarButton).Name;
            if (label == "addBtn")
            {
                Frame.Navigate(typeof(EditPage));
                return;
            }
            if (label == "enterEditBtn")
            {
                ViewModel.InEdit = true;
                taskBox.SelectionMode = ListViewSelectionMode.Multiple;
                return;
            }
            if (label == "cancelMode")
            {
                ViewModel.InEdit = false;
                taskBox.SelectedIndex = -1;
                taskBox.SelectionMode = ListViewSelectionMode.Single;
                return;
            }
            if (taskBox.SelectedItems.Count < 1)
            {
                ViewModel.Warning(App.GetString("task_selected_error"));
                return;
            }
            var items = new int[taskBox.SelectedItems.Count];
            for (int i = 0; i < taskBox.SelectedItems.Count; i++)
            {
                items[i] = (taskBox.SelectedItems[i] as TaskItem).Id;
            }
            if (label == "stopTask")
            {
                _ = ViewModel.StopTaskAsync(items);
                return;
            }
            _ = ViewModel.AddTaskAsync(items);
        }

        private void taskBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.InEdit)
            {
                return;
            }
            var item = (sender as ListView).SelectedItem as TaskItem;
            if (item == null)
            {
                return;
            }
            Frame.Navigate(typeof(EditPage), item.Id);
        }

        private void statusBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var status = (int)(statusBar.SelectedItem as TabItem).Value;
            ViewModel.Load(status);
            ViewModel.InEdit = false;
            taskBox.SelectedIndex = -1;
            taskBox.SelectionMode = ListViewSelectionMode.Single;
            enterEditBtn.Visibility = status < 2 ? Visibility.Visible : Visibility.Collapsed;
        }

        
    }
}
