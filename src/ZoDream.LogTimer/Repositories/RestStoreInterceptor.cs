using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Services;
using ZoDream.Shared.Http;
using ZoDream.Shared.Loggers;

namespace ZoDream.LogTimer.Repositories
{
    internal class RestStoreInterceptor(string apiEndpoint, string appId, string secret) : RestInterceptor(apiEndpoint, appId, secret)
    {
        public override string Token => Ioc.Default.GetService<IAuthService>()?.Token ?? string.Empty;

        public override HttpException ResponseFailure(HttpException ex)
        {
            if (ex.Code == 401)
            {
                Ioc.Default.GetService<IAuthService>()?.LogoutAsync();
            }
            Ioc.Default.GetService<ILogger>()?.Error(ex.Message);
            return ex;
        }
    }
}
