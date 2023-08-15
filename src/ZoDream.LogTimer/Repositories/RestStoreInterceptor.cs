using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Http;

namespace ZoDream.LogTimer.Repositories
{
    internal class RestStoreInterceptor : RestInterceptor
    {
        public RestStoreInterceptor(string apiEndpoint, string appId, string secret) : base(apiEndpoint, appId, secret)
        {
        }

        public override string Token => App.Store.Auth.Token;

        public override HttpException ResponseFailure(HttpException ex)
        {
            if (ex.Code == 401)
            {
                App.Store.Auth.LogoutAsync();
            }
            App.ViewModel.Logger.Error(ex.Message);
            return ex;
        }
    }
}
