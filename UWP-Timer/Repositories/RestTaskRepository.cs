using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;
using UWP_Timer.Repositories.Rest;
using Windows.Data.Json;

namespace UWP_Timer.Repositories
{
    /// <summary>
    /// 任务
    /// </summary>
    public class RestTaskRepository
    {
        public RestTaskRepository(RestRequest client)
        {
            http = client;
        }
        private readonly RestRequest http;
        /// <summary>
        /// 获取今天任务
        /// </summary>
        /// <param name="form"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<Page<TaskDay>> GetTaskDayListAsync(SearchForm form, Action<HttpException> action = null)
            => await http.GetAsync<Page<TaskDay>>("task/home/today", form.ToQueries(), action);
        /// <summary>
        /// 获取今天任务详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<TaskDay> GetTaskDayDetailAsync(int id, Action<HttpException> action = null)
            => await http.GetAsync<TaskDay>("task/home/detail_day", "id", id, action);
        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="form"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<Page<TaskItem>> GetTaskAsync(SearchForm form, Action<HttpException> action = null)
            => await http.GetAsync<Page<TaskItem>>("task", form.ToQueries(), action);
        /// <summary>
        /// 获取任务执行记录
        /// </summary>
        /// <param name="form"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<Page<TaskLog>> GetTaskReviewAsync(SearchForm form, Action<HttpException> action = null)
            => await http.GetAsync<Page<TaskLog>>("task/record", form.ToQueries(), action);
        /// <summary>
        /// 获取任务详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<TaskItem> GetTaskDetailAsync(int id, Action<HttpException> action = null)
            => await http.GetAsync<TaskItem>("task/home/detail", "id", id, action);
        /// <summary>
        /// 保存任务
        /// </summary>
        /// <param name="item"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<TaskItem> SaveTaskAsync(TaskForm item, Action<HttpException> action = null)
            => await http.PostAsync<TaskForm, TaskItem>("task/home/save", item, action);
        /// <summary>
        /// 批量添加任务到今日
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<ResponseDataOne<bool>> BatchAddTaskAsync(int id, Action<HttpException> action = null)
            => await http.PostAsync<ResponseDataOne<bool>>("task/home/batch_add", "{\"id\":" + id + "}", action);

        public async Task<TaskDay> AddTodayTaskAsync(int id, Action<HttpException> action = null)
            => await http.PostAsync<TaskDay>("task/home/save_day", "{\"task_id\":" + id + "}", action);

        /// <summary>
        /// 批量添加任务到今日
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<ResponseDataOne<bool>> BatchAddTaskAsync(int[] id, Action<HttpException> action = null)
        {
            return await http.PostAsync<ResponseDataOne<bool>>("task/home/batch_add", "{\"id\":[" + string.Join(',', id) + "]}", action);
        }
        /// <summary>
        /// 批量结束并完成任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<ResponseDataOne<bool>> BatchStopTaskAsync(int id, Action<HttpException> action = null)
            => await http.PostAsync<ResponseDataOne<bool>>("task/home/batch_stop", "{\"id\":"+id+"}", action);
        /// <summary>
        /// 批量结束并完成任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<ResponseDataOne<bool>> BatchStopTaskAsync(int[] id, Action<HttpException> action = null)
            => await http.PostAsync<ResponseDataOne<bool>>("task/home/batch_stop", "{\"id\":[" + string.Join(',', id) + "]}", action);
        /// <summary>
        /// 开始任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<TaskDay> PlayTaskAsync(int id, Action<HttpException> action = null)
            => await http.PostAsync<TaskDay>("task/home/play", new Dictionary<string, string>()
            {
                {"id", id.ToString() }
            }, action);
        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<TaskDay> PauseTaskAsync(int id, Action<HttpException> action = null)
            => await http.PostAsync<TaskDay>("task/home/pause", new Dictionary<string, string>()
            {
                {"id", id.ToString() }
            }, action);
        /// <summary>
        /// 终止任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<TaskDay> StopTaskAsync(int id, Action<HttpException> action = null)
            => await http.PostAsync<TaskDay>("task/home/stop", new Dictionary<string, string>()
            {
                {"id", id.ToString() }
            }, action);
        /// <summary>
        /// 验证任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<TaskDay> CheckTaskAsync(int id, Action<HttpException> action = null)
            => await http.PostAsync<TaskDay>("task/home/check", new Dictionary<string, string>()
            {
                {"id", id.ToString() }
            }, action);

        public async Task<Page<TaskShare>> GetShareListAsync(Queries form, Action<HttpException> action = null)
            => await http.GetAsync<Page<TaskShare>>("task/share", form, action);

        public async Task<Page<TaskShare>> GetShareMyAsync(Queries form, Action<HttpException> action = null)
          => await http.GetAsync<Page<TaskShare>>("task/share/my", form, action);

        public async Task<TaskShare> ShareCreateAsync(TaskShareForm item, Action<HttpException> action = null)
            => await http.PostAsync<TaskShareForm, TaskShare>("task/share/create", item, action);

        /// <summary>
        /// 快速添加任务
        /// </summary>
        /// <param name="item"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<TaskDay> FastCreateAsync(TaskForm item, Action<HttpException> action = null)
            => await http.PostAsync<TaskForm, TaskDay>("task/home/fast_create", item, action);
    }
}
