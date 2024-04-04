using CommunityToolkit.Mvvm.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories.Models;
using ZoDream.LogTimer.Services;
using ZoDream.Shared.Http;

namespace ZoDream.LogTimer.Repositories
{
    public class RestSiteRepository(RestRequest client)
    {
        private readonly RestRequest Client = client;

        public async Task<IList<EmojiCategory>> GetEmojiAsync()
        {
            var data = await Ioc.Default.GetService<ICacheService>()?.GetOrSetAsync("emoji.json", async () =>
            {
                return await Client.GetAsync<ResponseData<EmojiCategory>>("seo/emoji");
            });
            return data?.Data;
        }

        public async Task<BatchData> BatchAsync(IDictionary<string, object> data)
        {
            return await Client.PostAsync<BatchData>("open/batch", data);
        }
    }
}
