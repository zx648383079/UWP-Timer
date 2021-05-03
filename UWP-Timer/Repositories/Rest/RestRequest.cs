using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Utils;
using Windows.System.UserProfile;
using Windows.Web.Http;

namespace UWP_Timer.Repositories.Rest
{
    public class RestRequest
    {
        public RestRequest(RequestInterceptor interceptor)
        {
            Interceptor = interceptor;
        }

        private readonly RequestInterceptor Interceptor;

        /// <summary>
        /// Makes an HTTP GET request to the given controller and returns the deserialized response content.
        /// </summary>
        public async Task<TResult> GetAsync<TResult>(string controller, Action<HttpException> action = null)
        {
            using (var client = CreateHttp())
            {
                var obj = await client.AppendPath(controller).ExecuteAsync<TResult>(action);
                return obj;
            }
        }

        public async Task<TResult> GetAsync<TResult>(string controller, string key, object value, Action<HttpException> action = null)
        {
            using (var client = CreateHttp())
            {
                var obj = await client.AppendPath(controller).AddQuery(key, value.ToString()).ExecuteAsync<TResult>(action);
                return obj;
            }
        }

        public async Task<TResult> GetAsync<TResult>(string controller, Dictionary<string, string> parameters, Action<HttpException> action = null)
        {
            using (var client = CreateHttp())
            {
                var obj = await client.AppendPath(controller).AddQueries(parameters).ExecuteAsync<TResult>(action);
                return obj;
            }
        }

        public async Task<TResult> GetAsync<TResult>(string controller, object parameters, Action<HttpException> action = null)
        {
            using (var client = CreateHttp())
            {
                var obj = await client.AppendPath(controller).AddQueries(Arr.ToMap(parameters)).ExecuteAsync<TResult>(action);
                return obj;
            }
        }

        /// <summary>
        /// Makes an HTTP POST request to the given controller with the given object as the body.
        /// Returns the deserialized response content.
        /// </summary>
        public async Task<TResult> PostAsync<TRequest, TResult>(string controller, TRequest body, Action<HttpException> action = null)
        {
            using (var client = CreatePostHttp())
            {
                var obj = await client.AppendPath(controller).SetBody(new JsonStringContent(body)).ExecuteAsync<TResult>(action);
                return obj;
            }
        }

        public async Task<TResult> PostAsync<TResult>(string controller, object body, Action<HttpException> action = null)
        {
            using (var client = CreatePostHttp())
            {
                var obj = await client.AppendPath(controller).SetBody(new JsonStringContent(body)).ExecuteAsync<TResult>(action);
                return obj;
            }
        }

        public async Task<TResult> PostAsync<TResult>(string controller, Dictionary<string, string> body, Action<HttpException> action = null)
        {
            using (var client = CreatePostHttp())
            {
                var obj = await client.AppendPath(controller).SetBody(new JsonStringContent(body)).ExecuteAsync<TResult>(action);
                return obj;
            }
        }

        public async Task<TResult> PostAsync<TResult>(string controller, IHttpContent body, Action<HttpException> action = null)
        {
            using (var client = CreatePostHttp())
            {
                var obj = await client.AppendPath(controller).SetBody(body).ExecuteAsync<TResult>(action);
                return obj;
            }
        }

        public async Task<TResult> PostAsync<TResult>(string controller, string body, Action<HttpException> action = null)
        {
            using (var client = CreatePostHttp())
            {
                var obj = await client.AppendPath(controller).SetBody(JsonStringContent.ParseJson(body)).ExecuteAsync<TResult>(action);
                return obj;
            }
        }

        /// <summary>
        /// Makes an HTTP DELETE request to the given controller and includes all the given
        /// object's properties as URL parameters. Returns the deserialized response content.
        /// </summary>
        public async Task<TResult> DeleteAsync<TResult>(string controller, uint objectId, Action<HttpException> action = null)
        {
            using (var client = CreateHttp())
            {
                client.Method = HttpMethod.Delete;
                return await client.AppendPath($"{controller}/{objectId}").ExecuteAsync<TResult>(action);
            }
        }

        public async Task<TResult> PutAsync<TRequest, TResult>(string controller, TRequest body, Action<HttpException> action = null)
        {
            using (var client = CreatePostHttp())
            {
                client.AppendPath(controller).SetBody(new JsonStringContent(body));
                client.Method = HttpMethod.Put;
                var obj = await client.ExecuteAsync<TResult>(action);
                return obj;
            }
        }

        /// <summary>
        /// Constructs the base HTTP client, including correct authorization and API version headers.
        /// </summary>
        public RestClient CreateHttp()
        {
            return Interceptor.Request(new RestClient());
        }

        public RestClient CreatePostHttp()
        {
            var client = CreateHttp();
            client.Method = HttpMethod.Post;
            return client;
        }
    }
}
