using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static ZXing.QrCode.Internal.Mode;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Services;

namespace ZoDream.LogTimer.ViewModels
{
    internal class FeedbackViewModel : ObservableObject
    {
        public FeedbackViewModel()
        {
            SubmitCommand = new RelayCommand(TapSubmit);
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        private readonly INotificationService _notify = App.GetService<INotificationService>();
        private readonly NavigationService _router = App.GetService<NavigationService>();
        private readonly RestAccountRepository _api = App.GetService<RestAccountRepository>();

        private string email = string.Empty;

        public string Email {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string name = string.Empty;

        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string phone = string.Empty;

        public string Phone {
            get => phone;
            set => SetProperty(ref phone, value);
        }

        private string content = string.Empty;

        public string Content {
            get => content;
            set => SetProperty(ref content, value);
        }




        public ICommand SubmitCommand { get; private set; }

        private void TapSubmit()
        {
            var form = new Feedback()
            {
                Email = Email,
                Name = Name,
                Phone = Phone,
                Content = Content
            };
            if (string.IsNullOrWhiteSpace(form.Name))
            {
                _notify.Warning(App.GetString("fk_name_error"));
                return;
            }
            if (string.IsNullOrWhiteSpace(form.Content))
            {
                _notify.Warning(App.GetString("fk_content_error"));
                return;
            }
            _ = SaveAsync(form);
        }
        private async Task SaveAsync(Feedback form)
        {
            _notify.Loading(true);
            var data = await _api.SaveFeedbackAsync(form, res => {
                _app.DispatcherQueue.TryEnqueue(() => {
                    _notify.Error(res.Message);
                });
            });
            _app.DispatcherQueue.TryEnqueue(() => {
                _notify.Loading(false);
                _notify.Success(App.GetString("fk_save_success"));
                _router.GoBack();
            });
        }

    }
}
