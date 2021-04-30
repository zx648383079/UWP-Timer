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
        private readonly RestRequest http = new RestRequest();
        /// <summary>
        /// 文章列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Page<MicroItem>> GetPageAsync(SearchForm data)
        {
            return await http.GetAsync<Page<MicroItem>>("micro", data.ToQueries());
        }
        

    }
}
