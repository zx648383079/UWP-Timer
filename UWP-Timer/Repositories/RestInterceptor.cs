using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Repositories.Rest;
using UWP_Timer.Utils;
using Windows.System.UserProfile;

namespace UWP_Timer.Repositories
{
    public class RestInterceptor: RequestInterceptor
    {
        public string ApiEndpoint { get; private set; }

        public string AppId { get; private set; }

        public string Secret { get; private set; }

        public RestInterceptor(string apiEndpoint, string appId, string secret)
        {
            ApiEndpoint = apiEndpoint;
            AppId = appId;
            Secret = secret;
        }

        public string Token
        {
            get
            {
                return Constants.Token;
            }
        }

        public RestClient Request(RestClient client)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var headers = new Dictionary<string, string>
            {
                { "Date", timestamp },
                { "Content-Type", "application/vnd.api+json" },
                { "Accept", "application/json" },
                {  "HTTP_USER_AGENT", "zodream/5.0 UWPTimer/2.0" }
            };
            if (GlobalizationPreferences.Languages.Count > 0)
            {
                headers.Add("Accept-Language", GlobalizationPreferences.Languages[0]);
            }
            if (!string.IsNullOrEmpty(Constants.Token))
            {
                headers.Add("Authorization", "Bearer " + Constants.Token);
            }
            client.BaseUri = ApiEndpoint;
            client.AddQuery("appid", AppId).AddQuery("timestamp", timestamp)
                .AddQuery("sign", Str.MD5Encode(AppId + timestamp + Secret)).AddHeaders(headers);
            return client;
        }

        public T Response<T>(object data)
        {
            return (T)data;
        }
    }
}
