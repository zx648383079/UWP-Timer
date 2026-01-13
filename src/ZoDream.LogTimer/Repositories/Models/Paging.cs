using System.Collections.Generic;

namespace ZoDream.LogTimer.Repositories.Models
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class Paging
    {
        public int Limit { get; set; }

        public int Offset { get; set; }

        public int Total { get; set; }

        public bool More { get; set; }
    }

    public class BaseResponse
    {
        public string Appid { get; set; }

        public string Sign { get; set; }

        public string SignType { get; set; }

        public string Timestamp { get; set; }

        public string Encrypt { get; set; }

        public string EncryptType { get; set; }
    }

    public class ResponseData<T> : BaseResponse
    {
        public IList<T> Data { get; set; }
    }

    public class ResponseDataOne<T> : BaseResponse
    {
        public T Data { get; set; }
    }

    public class Page<T> : ResponseData<T>
    {
        public Paging Paging { get; set; }
    }
}
