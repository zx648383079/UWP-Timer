using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Utils;
using ZoDream.Shared.Http;

namespace ZoDream.LogTimer.Repositories
{
    public class RestSiteRepository
    {
        public RestSiteRepository(RestRequest client)
        {
            Client = client;
        }
        private readonly RestRequest Client;

        public async Task<IList<EmojiCategory>> GetEmojiAsync()
        {
            var data = await Cache.GetOrSetAsync<ResponseData<EmojiCategory>>("emoji.json", async () =>
            {
                return await Client.GetAsync<string>("seo/emoji");
            });
            return data?.Data;
        }
    }
}
