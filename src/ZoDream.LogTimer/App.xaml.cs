using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Services;
using ZoDream.LogTimer.ViewModels;
using ZoDream.Shared.Http;
using ZoDream.Shared.Loggers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            InitializeServices();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
        }

        private Window? _window;
        private static void InitializeServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton(new NavigationService());
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<ISettingService, SettingService>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IThemeService, ThemeService>();
            services.AddSingleton<ICacheService, FileCacheService>();
            services.AddSingleton<ILogger, EventLogger>();
            services.AddSingleton<ILocaleService, ResourceLocaleService>();
            services.AddSingleton<Deeplink>();
            services.AddSingleton<ShareService>();
            services.AddSingleton<TaskService>();
            services.AddScoped<RestAccountRepository>();
            services.AddScoped<RestArticleRepository>();
            services.AddScoped<RestAuthorizeRepository>();
            services.AddScoped<RestBulletinRepository>();
            services.AddScoped<RestCheckInRepository>();
            services.AddScoped<RestFileRepository>();
            services.AddScoped<RestMicroRepository>();
            services.AddScoped<RestSiteRepository>();
            services.AddScoped<RestTaskRepository>();
            services.AddScoped<RestUserRepository>();

            var app = new AppViewModel();
            services.AddSingleton(app);
            services.AddSingleton(_ => {
                var interceptor = new RestStoreInterceptor(Constants.ApiEndpoint, Constants.AppId, Constants.Secret);
                return new RestRequest(interceptor);
            });
            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
            app.LoadAsync();
        }

        public static T GetService<T>()
        {
            return Ioc.Default.GetService<T>();
        }

        public static string GetString(string key)
        {
            return GetService<ILocaleService>().Get(key);
        }
    }
}
