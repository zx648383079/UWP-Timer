using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace UWP_Timer.Utils
{
    public static class Share
    {

        public static async Task EncodeAsync(ShareData data)
        {
            var str = JsonConvert.SerializeObject(data);
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("share.zodream", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, str);
            await Windows.System.Launcher.LaunchFileAsync(file);
        }

        public static async Task DecodeAsync(Frame frame, IStorageFile file)
        {
            if (file == null)
            {
                return;
            }
            var str = await FileIO.ReadTextAsync(file as IStorageFile);
            var data = JsonConvert.DeserializeObject<ShareData>(str);
            frame.Navigate(typeof(Views.Micro.SharePage), data);
        }
    }

    public class ShareData
    {
        public string Appid { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Url { get; set; }

        public string Sharesource { get; set; }

        /// <summary>
        /// base64编码的图片
        /// </summary>
        public IList<string> Pics { get; set; }
    }
}
