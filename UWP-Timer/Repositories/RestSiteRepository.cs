using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;
using UWP_Timer.Repositories.Rest;
using UWP_Timer.Utils;

namespace UWP_Timer.Repositories
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
