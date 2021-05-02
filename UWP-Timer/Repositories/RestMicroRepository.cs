using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;
using UWP_Timer.Repositories.Rest;

namespace UWP_Timer.Repositories
{
    /// <summary>
    /// 文章
    /// </summary>
    public class RestMicroRepository
    {
        public RestMicroRepository(RestRequest client)
        {
            http = client;
        }
        private readonly RestRequest http;
        /// <summary>
        /// 文章列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Page<MicroItem>> GetPageAsync(SearchForm data)
        {
            return await http.GetAsync<Page<MicroItem>>("micro", data.ToQueries());
        }

        public async Task<MicroItem> GetAsync(int id)
        {
            return await http.GetAsync<MicroItem>("micro/home/detail", "id", id);
        }

        public async Task<Page<CommentBase>> GetCommentAsync(SearchForm data)
        {
            return await http.GetAsync<Page<CommentBase>>("micro/comment", data.ToQueries());
        }

        public async Task<MicroItem> CreateAsync(object data)
        {
            return await http.PostAsync<MicroItem>("micro/home/create", data);
        }

    }
}
