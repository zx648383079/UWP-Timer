using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Repositories.Rest;
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
                .AddQuery("sign", EncryptWithMD5(AppId + timestamp + Secret)).AddHeaders(headers);
            return client;
        }

        public T Response<T>(object data)
        {
            return (T)data;
        }

        public static string EncryptWithMD5(string source)
        {
            var sor = Encoding.UTF8.GetBytes(source);
            var md5 = MD5.Create();
            var result = md5.ComputeHash(sor);
            md5.Dispose();
            var strbul = new StringBuilder(40);
            for (int i = 0; i < result.Length; i++)
            {
                strbul.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

            }
            return strbul.ToString();
        }
    }
}
