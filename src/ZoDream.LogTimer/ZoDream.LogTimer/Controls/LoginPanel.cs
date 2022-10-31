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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    public sealed class LoginPanel : Control
    {
        const string FirstTabName = "PART_FirstTab";
        const string LastTabName = "PART_LastTab";
        const string QrBtnName = "PART_QrBtn";

        const string MobilePasswordBtnName = "PART_MobilePasswordBtn";
        const string LoginBtnName = "PART_LoginBtn";
        const string CodeSendBtnName = "PART_CodeSendBtn";
        const string MobileBackBtnName = "PART_MobileBackBtn";

        const string Login2BtnName = "PART_Login2Btn";
        const string Login3BtnName = "PART_Login3Btn";

        const string QrRefreshBtnName = "PART_QrRefreshBtn";

        public LoginPanel()
        {
            this.DefaultStyleKey = typeof(LoginPanel);
        }

        #region 对外设置


        public Style HeaderStyle
        {
            get { return (Style)GetValue(HeaderStyleProperty); }
            set { SetValue(HeaderStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderStyleProperty =
            DependencyProperty.Register("HeaderStyle", typeof(Style), typeof(LoginPanel), 
                new PropertyMetadata(null, (d, s) =>
                {
                    (d as LoginPanel).RefreshHeaderStyle();
                }));



        public Style HeaderActiveStyle
        {
            get { return (Style)GetValue(HeaderActiveStyleProperty); }
            set { SetValue(HeaderActiveStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderActiveStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderActiveStyleProperty =
            DependencyProperty.Register("HeaderActiveStyle", typeof(Style), typeof(LoginPanel), 
                new PropertyMetadata(null, (d, s) =>
                {
                    (d as LoginPanel).RefreshHeaderStyle();
                }));



        public LoginPanelMode PanelMode
        {
            get { return (LoginPanelMode)GetValue(PanelModeProperty); }
            set { SetValue(PanelModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PanelMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PanelModeProperty =
            DependencyProperty.Register("PanelMode", typeof(LoginPanelMode), typeof(LoginPanel), 
                new PropertyMetadata(LoginPanelMode.MobileCode, (d, e) =>
                {
                    (d as LoginPanel).RefreshMode();
                }));



        public string Mobile
        {
            get { return (string)GetValue(MobileProperty); }
            set { SetValue(MobileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mobile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MobileProperty =
            DependencyProperty.Register("Mobile", typeof(string), typeof(LoginPanel), new PropertyMetadata(default(string)));



        public string Code
        {
            get { return (string)GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Code.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register("Code", typeof(string), typeof(LoginPanel), new PropertyMetadata(default(string)));




        public bool IsAgree
        {
            get { return (bool)GetValue(IsAgreeProperty); }
            set { SetValue(IsAgreeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAgree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAgreeProperty =
            DependencyProperty.Register("IsAgree", typeof(bool), typeof(LoginPanel), new PropertyMetadata(true));



        public string Email
        {
            get { return (string)GetValue(EmailProperty); }
            set { SetValue(EmailProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Email.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register("Email", typeof(string), typeof(LoginPanel), new PropertyMetadata(default(string)));



        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(LoginPanel), new PropertyMetadata(default(string)));



        #endregion

        #region 控制变化



        public Style HeaderFirstStyle
        {
            get { return (Style)GetValue(HeaderFirstStyleProperty); }
            set { SetValue(HeaderFirstStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderFirstStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderFirstStyleProperty =
            DependencyProperty.Register("HeaderFirstStyle", typeof(Style), typeof(LoginPanel), new PropertyMetadata(null));



        public Style HeaderLastStyle
        {
            get { return (Style)GetValue(HeaderLastStyleProperty); }
            set { SetValue(HeaderLastStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderLastStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderLastStyleProperty =
            DependencyProperty.Register("HeaderLastStyle", typeof(Style), typeof(LoginPanel), new PropertyMetadata(null));




        public Visibility MobileCodeVisibility
        {
            get { return (Visibility)GetValue(MobileCodeVisibilityProperty); }
            set { SetValue(MobileCodeVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MobileCodeVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MobileCodeVisibilityProperty =
            DependencyProperty.Register("MobileCodeVisibility", typeof(Visibility), typeof(LoginPanel), new PropertyMetadata(Visibility.Visible));

        public Visibility MobilePasswordVisibility
        {
            get { return (Visibility)GetValue(MobilePasswordVisibilityProperty); }
            set { SetValue(MobilePasswordVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MobileCodeVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MobilePasswordVisibilityProperty =
            DependencyProperty.Register("MobilePasswordVisibility", typeof(Visibility), typeof(LoginPanel), new PropertyMetadata(Visibility.Collapsed));

        public Visibility AccountVisibility
        {
            get { return (Visibility)GetValue(AccountVisibilityProperty); }
            set { SetValue(AccountVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MobileCodeVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AccountVisibilityProperty =
            DependencyProperty.Register("AccountVisibility", typeof(Visibility), typeof(LoginPanel), new PropertyMetadata(Visibility.Collapsed));

        public Visibility QrVisibility
        {
            get { return (Visibility)GetValue(QrVisibilityProperty); }
            set { SetValue(QrVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MobileCodeVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QrVisibilityProperty =
            DependencyProperty.Register("QrVisibility", typeof(Visibility), typeof(LoginPanel), new PropertyMetadata(Visibility.Collapsed));



        public string QrIcon
        {
            get { return (string)GetValue(QrIconProperty); }
            set { SetValue(QrIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for QrIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QrIconProperty =
            DependencyProperty.Register("QrIcon", typeof(string), typeof(LoginPanel), new PropertyMetadata(default(string)));



        public string QrTip
        {
            get { return (string)GetValue(QrTipProperty); }
            set { SetValue(QrTipProperty, value); }
        }

        // Using a DependencyProperty as the backing store for QrTip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QrTipProperty =
            DependencyProperty.Register("QrTip", typeof(string), typeof(LoginPanel), new PropertyMetadata(default(string)));



        public string QrSource
        {
            get { return (string)GetValue(QrSourceProperty); }
            set { SetValue(QrSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for QrSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QrSourceProperty =
            DependencyProperty.Register("QrSource", typeof(string), typeof(LoginPanel), new PropertyMetadata(default(string)));




        #endregion

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var firstTab = GetTemplateChild(FirstTabName) as FrameworkElement;
            var lastTab = GetTemplateChild(LastTabName) as FrameworkElement;
            var toMPBtn = GetTemplateChild(MobilePasswordBtnName) as FrameworkElement;
            var backBtn = GetTemplateChild(MobileBackBtnName) as FrameworkElement;
            var qrBtn = GetTemplateChild(QrBtnName) as FrameworkElement;
            var loginBtn = GetTemplateChild(LoginBtnName) as Control;
            var login2Btn = GetTemplateChild(Login2BtnName) as Control;
            var login3Btn = GetTemplateChild(Login3BtnName) as Control;
            if (firstTab != null)
            {
                firstTab.Tapped += (d, s) =>
                {
                    PanelMode = LoginPanelMode.MobileCode;
                };
            }
            if (lastTab != null)
            {
                lastTab.Tapped += (d, s) =>
                {
                    PanelMode = LoginPanelMode.Account;
                };
            }
            if (toMPBtn != null)
            {
                toMPBtn.Tapped += (d, s) =>
                {
                    PanelMode = LoginPanelMode.MobilePassword;
                };
            }
            if (backBtn != null)
            {
                backBtn.Tapped += (d, s) =>
                {
                    PanelMode = LoginPanelMode.MobileCode;
                };
            }
            if (qrBtn != null)
            {
                qrBtn.Tapped += (d, s) =>
                {
                    PanelMode = LoginPanelMode.Qr;
                };
            }
            if (loginBtn != null)
            {
                loginBtn.Tapped += LoginBtn_Tapped;
            }
            if (login2Btn != null)
            {
                login2Btn.Tapped += LoginBtn_Tapped;
            }
            if (login3Btn != null)
            {
                login3Btn.Tapped += LoginBtn_Tapped;
            }
        }

        private void LoginBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
        }

        public void RefreshMode()
        {
            RefreshHeaderStyle();
            MobileCodeVisibility = PanelMode == LoginPanelMode.MobileCode ? Visibility.Visible : Visibility.Collapsed;
            MobilePasswordVisibility = PanelMode == LoginPanelMode.MobilePassword ? Visibility.Visible : Visibility.Collapsed;
            AccountVisibility = PanelMode == LoginPanelMode.Account ? Visibility.Visible : Visibility.Collapsed;
            QrVisibility = PanelMode == LoginPanelMode.Qr ? Visibility.Visible : Visibility.Collapsed;
        }

        private void RefreshHeaderStyle()
        {
            HeaderFirstStyle = PanelMode == LoginPanelMode.MobileCode ||
                PanelMode == LoginPanelMode.MobilePassword ? HeaderActiveStyle : HeaderStyle;
            HeaderLastStyle = PanelMode == LoginPanelMode.Account ? HeaderActiveStyle : HeaderStyle;
        }
    }

    public enum LoginPanelMode
    {
        MobileCode,
        MobilePassword,
        Account,
        Qr,
    }
}
