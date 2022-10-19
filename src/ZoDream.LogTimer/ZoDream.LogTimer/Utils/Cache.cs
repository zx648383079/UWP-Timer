using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ZoDream.LogTimer.Utils
{
    public static class Cache
    {
        public static async Task<T> GetOrSetAsync<T>(string name, Func<Task<string>> func)
        {
            var folder = ApplicationData.Current.LocalFolder;
            StorageFile file;
            try
            {
                file = await folder.GetFileAsync(name);
            }
            catch (Exception)
            {
                file = null;
            }
            string content;
            if (file != null)
            {
                content = await FileIO.ReadTextAsync(file);
            } else {
                content = await func.Invoke();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return (T)(object)null;
                }
                file = await folder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
                _ = FileIO.WriteTextAsync(file, content);
            }
            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception)
            {
                _ = file.DeleteAsync();
            }
            return (T)(object)null;
        }

        public static async Task ClearAsync()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var files = new string[] { };
            var items = await folder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName);
            foreach (var file in items)
            {
                if (Arr.Contain(file.Name, files))
                {
                    await file.DeleteAsync();
                }
            }
        }
    }
}
