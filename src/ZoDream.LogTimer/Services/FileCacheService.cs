using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;
using ZoDream.LogTimer.Utils;

namespace ZoDream.LogTimer.Services
{
    public class FileCacheService : ICacheService
    {

        private readonly StorageFolder RootFolder = ApplicationData.Current.LocalFolder;

        public async Task ClearAsync()
        {
            var files = Array.Empty<string>();
            var items = await RootFolder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName);
            foreach (var file in items)
            {
                if (Arr.Contain(file.Name, files))
                {
                    await file.DeleteAsync();
                }
            }
        }

        public async Task RemoveAsync<T>(string name)
        {
            try
            {
                var file = await RootFolder.GetFileAsync(name);
                await file.DeleteAsync();
            }
            catch (FileNotFoundException)
            {
            }
        }

        public async Task<T?> GetAsync<T>(string name)
        {
            try
            {
                var file = await RootFolder.GetFileAsync(name);
                return JsonSerializer.Deserialize<T>(await FileIO.ReadTextAsync(file));
            }
            catch (FileNotFoundException)
            {
            }
            return default;
        }

        public async Task<T?> GetOrSetAsync<T>(string name, Func<Task<T?>> func)
        {
            try
            {
                var file = await RootFolder.GetFileAsync(name);
                return JsonSerializer.Deserialize<T>(await FileIO.ReadTextAsync(file));
            }
            catch (FileNotFoundException)
            {
            }
            var data = await func.Invoke();
            if (data is null)
            {
                return data;
            }
            await SetAsync(name, data);
            return data;
        }

        public async Task SetAsync<T>(string name, T data)
        {
            var file = await RootFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, JsonSerializer.Serialize(data));
        }
    }
}
