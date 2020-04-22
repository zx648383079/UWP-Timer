﻿using System;
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
    public class RestArticleRepository
    {
        private readonly RestRequest http = new RestRequest();
        /// <summary>
        /// 文章列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Page<Article>> GetPageAsync(SearchForm data)
        {
            return await http.GetAsync<Page<Article>>("blog", data.ToQueries());
        }
        /// <summary>
        /// 获取文章分类
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseData<ArticleCategory>> GetCategoriesAsync()
        {
            return await http.GetAsync<ResponseData<ArticleCategory>>("blog/term");
        }
        /// <summary>
        /// 获取一篇文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Article> GetArticleAsync(int id)
        {
            return await http.GetAsync<Article>("blog", "id", id);
        }

        /// <summary>
        /// 获取搜索建议
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public async Task<ResponseData<Article>> GetSuggestionAsync(string keywords)
        {
            return await http.GetAsync<ResponseData<Article>>("blog/home/suggest", "keywords", keywords);
        }

    }
}
