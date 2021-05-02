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
    /// 签到
    /// </summary>
    public class RestCheckInRepository
    {
        public RestCheckInRepository(RestRequest client)
        {
            http = client;
        }
        private readonly RestRequest http;
        /// <summary>
        /// 判断今天是否签到
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<ResponseDataOne<CheckIn>> GetCanCheckInAsync(Action<HttpException> action = null)
            => await http.GetAsync<ResponseDataOne<CheckIn>>("checkin", action);
        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<ResponseDataOne<CheckIn>> CheckInAsync(Action<HttpException> action = null)
            => await http.GetAsync<ResponseDataOne<CheckIn>>("checkin/home/check_in", action);
        /// <summary>
        /// 获取当月签到情况
        /// </summary>
        /// <param name="month">月份 2020-01-01</param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<ResponseData<CheckIn>> GetMonthAsync(string month, Action<HttpException> action = null)
            => await http.GetAsync<ResponseData<CheckIn>>("checkin/home/month", "month", month, action);

        public async Task<CheckInBatch> BatchAsync(object data, Action<HttpException> action = null)
        {
            return await http.PostAsync<CheckInBatch>("checkin/batch", data, action);
        }
    }
}
