using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace ZoDream.LogTimer.Services
{
    public class ShareService(NavigationService navigation)
    {

        public async Task EncodeAsync(ShareData data)
        {
            var str = JsonSerializer.Serialize(data);
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("share.zodream", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, str);
            await Windows.System.Launcher.LaunchFileAsync(file);
        }

        public async Task DecodeAsync(IStorageFile? file)
        {
            if (file == null)
            {
                return;
            }
            var str = await FileIO.ReadTextAsync(file);
            var data = JsonSerializer.Deserialize<ShareData>(str);
            navigation.Navigate(typeof(Pages.Micro.SharePage), data);
        }
    }

    public class ShareData
    {
        public string Appid { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string Sharesource { get; set; } = string.Empty;

        /// <summary>
        /// base64编码的图片
        /// </summary>
        public IList<string> Pics { get; set; } = [];
    }
}
