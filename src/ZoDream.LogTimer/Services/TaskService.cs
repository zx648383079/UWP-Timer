using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;
using ZoDream.LogTimer.Utils;

namespace ZoDream.LogTimer.Services
{
    internal class TaskService(RestTaskRepository api, INotificationService notify)
    {
        public int Id => Today != null ? Today.Id : 0;

        public TaskDay? Today { get; set; }

        public bool Paused { get; private set; } = true;

        public int Duration => Today == null || Today.Task == null ? 0 : Today.Task.EveryTime;

        public int Current { get; private set; }

        private DateTime StartTime;
        private DispatcherTimer? Timer;

        public event TaskChangedEventHandler? TimeUpdated;
        public event TaskChangedEventHandler? PausedChanged;
        public event TaskChangedEventHandler? TaskChanged;


        public void SetTask(TaskDay data)
        {
            Today = data;
            Paused = data.Log == null || data.Status != 9;
            Current = Today == null || Today.Log == null ? 0 : Today.Log.Time;
            TaskChanged?.Invoke();
        }

        public void Play(TaskDay data)
        {
            SetTask(data);
            PausedChanged?.Invoke();
            TimeUpdated?.Invoke();
            if (!Paused)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }

        public async Task PlayAsync(int id)
        {
            var data = await api.PlayTaskAsync(id, res => {
                SynchronizationContext.Current?.Post(o => {
                    notify.Push(res.Message);
                }, null);
            });
            if (data == null)
            {
                return;
            }
            Play(data);
        }

        public async Task PlayAsync()
        {
            await PlayAsync(Id);
        }

        public async Task StopAsync()
        {
            var data = await api.StopTaskAsync(Id, res => {
                SynchronizationContext.Current?.Post(o => {
                    notify.Push(res.Message);
                }, null);
            });
            if (data == null)
            {
                return;
            }
            Today = data;
            Paused = true;
            PausedChanged?.Invoke();
            TimeUpdated?.Invoke();
            Stop();
        }

        public async Task PauseAsync()
        {
            var data = await api.PauseTaskAsync(Id, res => {
                SynchronizationContext.Current?.Post(o => {
                    notify.Push(res.Message);
                }, null);
            });
            if (data == null)
            {
                return;
            }
            Today = data;
            Paused = true;
            PausedChanged?.Invoke();
            TimeUpdated?.Invoke();
            Stop();
        }

        public async Task CheckAsync()
        {
            var data = await api.CheckTaskAsync(Id, res => {
                SynchronizationContext.Current?.Post(o => {
                    notify.Push(res.Message);
                }, null);
            });
            if (data == null || data.Id < 1)
            {
                //Thread.Sleep(1000);
                //_ = checkAsync();
                return;
            }
            Today = data;
            Paused = true;
            PausedChanged?.Invoke();
            TimeUpdated?.Invoke();
            Stop();
            Toast.ShowInfo(data.Tip);
            notify.Push(data.Tip);
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        private void Start()
        {
            StartTime = DateTime.Now.AddSeconds(-Current);
            if (Timer != null)
            {
                Timer.Start();
                return;
            }
            Timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
            Timer.Tick += new EventHandler<object>((sender, e) => {
                var diff = (DateTime.Now - StartTime).TotalSeconds;
                if (Duration > 0 && diff >= Duration * 60)
                {
                    Stop();
                    return;
                }
                Current = Convert.ToInt32(diff);
                TimeUpdated?.Invoke();
            });
            Timer.Start();
        }
        /// <summary>
        /// 停止计时
        /// </summary>
        private void Stop()
        {
            if (Timer == null)
            {
                return;
            }
            Timer.Stop();
            _ = CheckAsync();
            TimeUpdated?.Invoke();
        }
    }
}
