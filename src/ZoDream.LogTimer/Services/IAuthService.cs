using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;

namespace ZoDream.LogTimer.Services
{
    public interface IAuthService
    {
        public bool Authenticated { get; }

        public string Token { get;}
        public User? AuthenticatedUser { get; }

        public bool Authenticating { get; set; }

        public event AuthChangedEventHandler? AuthChanged;

        public void LoginAsync(string token, User user);
        public void LoginAsync(User user);

        public void LogoutAsync();
    }
}
