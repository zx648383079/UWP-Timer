using System.Text.Json;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;

namespace ZoDream.LogTimer.Services
{
    public class AuthService(ISettingService setting): IAuthService
    {
        /// <summary>
        /// token 保存键
        /// </summary>
        public const string TOKEN_KEY = "token";

        public const string USER_KEY = "user";

        public string Token { get; private set; } = setting.Get<string>(TOKEN_KEY) ?? string.Empty;

        public User? AuthenticatedUser { get; private set; }

        private bool isLoading = false;

        public bool Authenticating {
            get { return isLoading; }
            set {
                isLoading = value;
                if (value)
                {
                    AuthChanged?.Invoke();
                }
            }
        }


        public bool Authenticated => !Authenticating && !string.IsNullOrEmpty(Token);

        public event AuthChangedEventHandler? AuthChanged;

        public void LoginAsync(string token, User user)
        {
            Authenticating = false;
            AuthenticatedUser = user;
            Token = token;
            setting.Set(USER_KEY, JsonSerializer.Serialize(user));
            setting.Set(TOKEN_KEY, Token);
            AuthChanged?.Invoke();
        }

        public void LoginAsync(User user)
        {
            Authenticating = false;
            AuthenticatedUser = user;
            setting.Set(USER_KEY, JsonSerializer.Serialize(user));
            AuthChanged?.Invoke();
        }

        public void LogoutAsync()
        {
            Authenticating = false;
            if (string.IsNullOrEmpty(Token))
            {
                AuthChanged?.Invoke();
                return;
            }
            Token = string.Empty;
            AuthenticatedUser = null;
            setting.Remove(TOKEN_KEY);
            setting.Remove(USER_KEY);
            AuthChanged?.Invoke();
        }
    }
}
