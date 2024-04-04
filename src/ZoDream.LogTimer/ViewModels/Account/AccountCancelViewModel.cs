using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Repositories.Models;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    internal class AccountCancelViewModel: ObservableObject
    {

        public AccountCancelViewModel()
        {
            ConfirmCommand = new RelayCommand(TapConfirm);
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        private readonly INotificationService _notify = App.GetService<INotificationService>();
        private readonly NavigationService _router = App.GetService<NavigationService>();
        private readonly RestAccountRepository _api = App.GetService<RestAccountRepository>();
        private readonly IAuthService _auth = App.GetService<IAuthService>();

        private string[] reasonItems = [
            App.GetString("cancel_reason_1"),
            App.GetString("cancel_reason_2"),
            App.GetString("cancel_reason_3"),
            App.GetString("cancel_reason_4"),
        ];

        public string[] ReasonItems {
            get => reasonItems;
            set => SetProperty(ref reasonItems, value);
        }

        private string selectedItem = string.Empty;

        public string SelectedItem {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }


        public ICommand ConfirmCommand { get; private set; }

        private void TapConfirm()
        {
            var reason = SelectedItem;
            if (string.IsNullOrEmpty(reason))
            {
                _notify.Push(App.GetString("cancel_reason_error"));
                return;
            }
            _ = cancelAsync(new CancelForm()
            {
                Reason = reason
            });
        }

        private async Task cancelAsync(CancelForm form)
        {
            _notify.Loading(true);
            var data = await _api.CancelUserAsync(form, res => {
                _app.DispatcherQueue.TryEnqueue(() => {
                    _notify.Loading(false);
                    _notify.Error(res.Message);
                });

            });
            _app.DispatcherQueue.TryEnqueue(() => {
                _notify.Loading(false);
                if (data == null)
                {
                    return;
                }
                _auth.LogoutAsync();
                _notify.Success(App.GetString("cancel_success_tip"));
                _router.Navigate(typeof(Pages.Plan.TodayPage));
            });
        }

        public async Task ShowConfirm()
        {
            var dialog = new ContentDialog()
            {
                Content = App.GetString("cancel_confirm_content"),
                Title = App.GetString("cancel_confirm_title"),
                CloseButtonText = App.GetString("cancel_confirm_no"),
                PrimaryButtonText = App.GetString("cancel_confirm_yes"),
            };
            var result = await App.GetService<AppViewModel>().OpenDialogAsync(dialog);
            if (result == ContentDialogResult.None)
            {
                _router.GoBack();
            }
        }
    }
}
