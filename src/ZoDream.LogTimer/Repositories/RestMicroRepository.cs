﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories.Models;
using ZoDream.Shared.Http;

namespace ZoDream.LogTimer.Repositories
{
    /// <summary>
    /// 文章
    /// </summary>
    public class RestMicroRepository(RestRequest client)
    {
        private readonly RestRequest http = client;
        /// <summary>
        /// 文章列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Page<MicroItem>> GetPageAsync(MicroQueries data)
        {
            return await http.GetAsync<Page<MicroItem>>("micro", data);
        }

        public async Task<MicroItem> GetAsync(int id)
        {
            return await http.GetAsync<MicroItem>("micro/home/detail", "id", id);
        }

        public async Task<Page<CommentBase>> GetCommentAsync(MicroCommentQueries data)
        {
            return await http.GetAsync<Page<CommentBase>>("micro/comment", data);
        }

        public async Task<MicroItem> CreateAsync(MicroForm data)
        {
            return await http.PostAsync<MicroItem>("micro/home/create", data);
        }

        internal async Task<CommentBase> CreateCommentAsync(MicroCommentForm form)
        {
            return await http.PostAsync<CommentBase>("micro/comment/save", form);
        }

        public async Task<ResponseDataOne<bool>> ShareCheckAsync(MicroShareForm form)
        {
            return await http.PostAsync<ResponseDataOne<bool>>("micro/share", form);
        }

        public async Task<MicroItem> ShareSaveAsync(MicroShareForm form)
        {
            return await http.PostAsync<MicroItem>("micro/share/save", form);
        }
    }
}
