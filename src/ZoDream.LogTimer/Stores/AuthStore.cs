using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Utils;

namespace ZoDream.LogTimer.Stores
{
    internal class AuthStore
    {
        public string Token { get; set; } = string.Empty;

        public User User { get; set; }

        private bool isLoading = false;

        public bool IsLoading
        {
            get { return isLoading; }
            set {
                isLoading = value;
                if (value)
                {
                    AuthChanged?.Invoke();
                }
            }
        }


        public bool IsAuthenticated => !IsLoading && !string.IsNullOrEmpty(Token);

        public event AuthChangedEventHandler AuthChanged;

        public void LoginAsync(string token, User user)
        {
            IsLoading = false;
            User = user;
            Token = token;
            AppData.SetValue(Constants.USER_KEY, JsonConvert.SerializeObject(user));
            AppData.SetValue(Constants.TOKEN_KEY, Token);
            AuthChanged?.Invoke();
        }

        public void LoginAsync(User user)
        {
            IsLoading = false;
            User = user;
            AppData.SetValue(Constants.USER_KEY, JsonConvert.SerializeObject(user));
            AuthChanged?.Invoke();
        }

        public void LogoutAsync() 
        {
            IsLoading = false;
            if (string.IsNullOrEmpty(Token))
            {
                AuthChanged?.Invoke();
                return;
            }
            Token = string.Empty;
            User = null;
            AppData.Remove(Constants.TOKEN_KEY);
            AppData.Remove(Constants.USER_KEY);
            AuthChanged?.Invoke();
        }
        
    }
}
