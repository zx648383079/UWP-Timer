using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web.Http;
using ZoDream.Shared.Http;
using ZoDream.LogTimer.Repositories.Models;

namespace ZoDream.LogTimer.Repositories
{
    /// <summary>
    /// 用户
    /// </summary>
    public class RestUserRepository
    {
        public RestUserRepository(RestRequest client)
        {
            http = client;
        }
        private readonly RestRequest http;
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<User> GetProfileAsync(HttpExceptionFunc action = null) =>
            await http.GetAsync<User>("auth/user", action);
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="login"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<User> LoginAsync(LoginForm login, HttpExceptionFunc action = null) =>
            await http.PostAsync<LoginForm, User>("auth/login", login, action);
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="form"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<User> RegisterAsync(RegisterForm form, HttpExceptionFunc action = null) =>
            await http.PostAsync<RegisterForm, User>("auth/register", form, action);
        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<ResponseDataOne<bool>> LogoutAsync(HttpExceptionFunc action = null) =>
            await http.GetAsync<ResponseDataOne<bool>>("auth/logout", action);
        /// <summary>
        /// 发送找回密码邮件
        /// </summary>
        /// <param name="email"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<ResponseDataOne<bool>> SendFindEmailAsync(string email, HttpExceptionFunc action = null)
            => await http.PostAsync<ResponseDataOne<bool>>("auth/password/send_find_email", new Dictionary<string, string>() {
                { "email", email }
            }, action);
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="form"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<ResponseDataOne<bool>> ResetAsync(RegisterForm form, HttpExceptionFunc action = null)
            => await http.PostAsync<RegisterForm, ResponseDataOne<bool>>("auth/password/reset", form, action);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="form"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<ResponseDataOne<bool>> PasswordUpdateAsync(PasswordForm form, HttpExceptionFunc action = null)
            => await http.PutAsync<PasswordForm, ResponseDataOne<bool>>("auth/password/update", form, action);
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="form"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<User> UpdateAsync(ProfileForm form, HttpExceptionFunc action = null)
            => await http.PutAsync<ProfileForm, User>("auth/user/update", form, action);

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="file"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<User> UploadAvatarAsync(StorageFile file, HttpExceptionFunc action = null)
        {
            return await UploadAvatarAsync(new HttpStreamContent(await file.OpenReadAsync()), action);
        }

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<User> UploadAvatarAsync(IInputStream stream, HttpExceptionFunc action = null)
        {
            return await UploadAvatarAsync(new HttpStreamContent(stream), action);
        }

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<User> UploadAvatarAsync(HttpStreamContent stream, HttpExceptionFunc action = null)
        {
            var form = new HttpMultipartFormDataContent();
            form.Add(stream, "file", "avatar.png");
            return await http.PostAsync<User>("auth/user/avatar", form, action);
        }

    }
}
