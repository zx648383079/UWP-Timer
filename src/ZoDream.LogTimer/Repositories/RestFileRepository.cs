using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web.Http;
using ZoDream.Shared.Http;
using ZoDream.LogTimer.Repositories.Models;
using System.Text.Json;

namespace ZoDream.LogTimer.Repositories
{
    public class RestFileRepository(RestRequest client)
    {
        private readonly RestRequest Client = client;


        public async Task<UploadResult> UploadImageAsync(StorageFile file, HttpExceptionFunc action = null)
        {
            return await UploadAsync<UploadResult>("open/file/image", new HttpStreamContent(await file.OpenReadAsync()), file.Name, action);
        }

        public async Task<UploadResult> UploadVideoAsync(StorageFile file, HttpExceptionFunc action = null)
        {
            return await UploadAsync<UploadResult>("open/file/video", new HttpStreamContent(await file.OpenReadAsync()), file.Name, action);
        }

        public async Task<UploadResult> UploadAudioAsync(StorageFile file, HttpExceptionFunc action = null)
        {
            return await UploadAsync<UploadResult>("open/file/audio", new HttpStreamContent(await file.OpenReadAsync()), file.Name, action);
        }

        public async Task<UploadResult> UploadFileAsync(StorageFile file, HttpExceptionFunc action = null)
        {
            return await UploadAsync<UploadResult>("open/file", new HttpStreamContent(await file.OpenReadAsync()), file.Name, action);
        }

        public async Task<IEnumerable<UploadResult>> UploadImagesAsync(IEnumerable<StorageFile> files, HttpExceptionFunc action = null)
        {
            var form = new HttpMultipartFormDataContent();
            foreach (var file in files)
            {
                form.Add(new HttpStreamContent(await file.OpenReadAsync()), "file[]", file.Name);
            }
            var data = await UploadAsync<string>("open/file/image", form, action);
            if (data == null)
            {
                return null;
            }
            try
            {
                if (data.IndexOf("\"data\"") < 0)
                {
                    var res = JsonSerializer.Deserialize<UploadResult>(data);
                    return new UploadResult[] { res };
                } else
                {
                    var res = JsonSerializer.Deserialize<ResponseData<UploadResult>> (data);
                    return res.Data;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<T> UploadAsync<T>(string path, StorageFile file, HttpExceptionFunc action = null)
        {
            return await UploadAsync<T>(path, new HttpStreamContent(await file.OpenReadAsync()), file.Name, action);
        }

        public async Task<T> UploadAsync<T>(string path, IInputStream stream, string fileName, HttpExceptionFunc action = null)
        {
            return await UploadAsync<T>(path, new HttpStreamContent(stream), fileName, action);
        }

        public async Task<T> UploadAsync<T>(string path, HttpStreamContent stream, string fileName, HttpExceptionFunc action = null)
        {
            var form = new HttpMultipartFormDataContent();
            form.Add(stream, "file", fileName);
            return await UploadAsync<T>(path, form, action);
        }

        public async Task<T> UploadAsync<T>(string path, HttpMultipartFormDataContent form, HttpExceptionFunc action = null)
        {
            return await Client.PostAsync<T>(path, form, action);
        }
    }

    public class UploadResult
    {
        public string Url { get; set; }

        public string Thumb { get; set; }

        public int Size { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public string Original { get; set; }
    }
}
