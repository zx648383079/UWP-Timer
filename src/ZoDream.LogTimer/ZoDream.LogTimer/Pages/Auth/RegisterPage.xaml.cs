﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using ZoDream.LogTimer.Extensions;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Pages.Plan;
using ZoDream.LogTimer.Repositories;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Pages.Auth
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("logo");
            if (imageAnimation != null)
            {
                imageAnimation.TryStart(LogoImg);
            }
        }

        private void backBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void regBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var form = new RegisterForm()
            {
                Name = nameTb.Text.Trim(),
                Email = emailTb.Text,
                Password = pwdTb.Password,
                RePassword = rePwdTb.Password,
                Agree = agreeTb.IsChecked == true
            };
            if (!form.Agree)
            {
                _ = new MessageDialog(Constants.GetString("login_agreement_error")).ShowAsync();
                return;
            }
            if (!form.VerifyEmail())
            {
                _ = new MessageDialog(Constants.GetString("login_email_error")).ShowAsync();
                return;
            }
            if (!form.VerifyPassword())
            {
                _ = new MessageDialog(Constants.GetString("login_pwd_error")).ShowAsync();
                return;
            }
            if (form.Password != form.RePassword)
            {
                _ = new MessageDialog(Constants.GetString("login_re_pwd_error")).ShowAsync();
                return;
            }
            _ = Register(form);
        }

        private async Task Register(RegisterForm form)
        {
            regBtn.IsTapEnabled = false;
            App.ViewModel.IsLoading = true;
            var dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();
            var data = await App.Repository.User.RegisterAsync(form, async res =>
            {
                await dispatcherQueue.EnqueueAsync(() =>
                {
                    App.ViewModel.IsLoading = false;
                    _ = new MessageDialog(res.Message).ShowAsync();
                });

            });
            await dispatcherQueue.EnqueueAsync(() =>
            {
                regBtn.IsTapEnabled = true;
                App.ViewModel.IsLoading = false;
                if (data == null)
                {
                    return;
                }
                App.Login(data);
                _ = new MessageDialog(Constants.GetString("login_reg_success_tip")).ShowAsync();
                Frame.Navigate(typeof(TodayPage));
            });

        }

        private void pwdTb_GotFocus(object sender, RoutedEventArgs e)
        {
            LogoImg.Status = Controls.LogoStatus.INIT;
        }

        private void pwdTb_LostFocus(object sender, RoutedEventArgs e)
        {
            LogoImg.Status = Controls.LogoStatus.NONE;
        }
    }
}