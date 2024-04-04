using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.LogTimer.Models;
using ZoDream.LogTimer.Repositories;

namespace ZoDream.LogTimer.ViewModels
{
    public class CheckInViewModel: ObservableObject
    {

        public CheckInViewModel()
        {
            PreviousCommand = new RelayCommand(PreviousMonth);
            NextCommand = new RelayCommand(NextMonth);
            CheckCommand = new RelayCommand(TapCheck);
            _ = CanCheck();
            RefreshDay();
        }

        private readonly RestCheckInRepository _api = App.GetService<RestCheckInRepository>();

        private string month;

        public string Month
        {
            get => month;
            set => SetProperty(ref month, value);
        }

        private bool isChecked = false;

        public bool IsChecked
        {
            get => isChecked;
            set => SetProperty(ref isChecked, value);
        }

        private string tip;

        public string Tip
        {
            get => tip;
            set => SetProperty(ref tip, value);
        }


        private ObservableCollection<DayItem> days = [];

        public ObservableCollection<DayItem> Days
        {
            get => days;
            set => SetProperty(ref days, value);
        }

        public ICommand PreviousCommand {  get; private set; }
        public ICommand NextCommand {  get; private set; }
        public ICommand CheckCommand {  get; private set; }

        private void TapCheck()
        {
            _ = CheckTodayAsync();
        }

        public async Task CanCheck()
        {
            var data = await _api.GetCanCheckInAsync();
            SynchronizationContext.Current.Post(o =>
            {
                if (data == null || data.Data == null)
                {
                    IsChecked = false;
                    Tip = String.Empty;
                    return;
                }
                IsChecked = true;
                Tip = App.GetString("check_in_tip").Replace("{day}", data.Data.Running.ToString());
            }, null);
        }

        public void RefreshDay()
        {
            SetMonth(DateTime.Now.AddDays(1 - DateTime.Now.Day));
        }

        public DateTime GetMonth()
        {
            return DateTime.Parse(Month + "-01");
        }

        public void SetMonth(DateTime date)
        {
            Month = date.Year + "-" + date.Month.ToString("00");
            days.Clear();
            var start = (int)date.DayOfWeek;
            var count = date.AddMonths(1).AddDays(-1).Day;
            start = start > 0 ? start - 1 : 6;
            for (int i = 0; i < count + start; i++)
            {
                days.Add(new DayItem()
                {
                    Day = i >= start ? i - start + 1 : 0,
                    IsChecked = false
                });
            }
            _ = GetMonthChecked();
        }

        private async Task GetMonthChecked()
        {
            var data = await _api.GetMonthAsync(Month);
            if (data == null || data.Data == null || data.Data.Count < 1)
            {
                return;
            }
            var items = new int[data.Data.Count];
            var i = 0;
            foreach (var item in data.Data)
            {
                items[i++] = DateTime.Parse(item.CreatedAt).Day;
            }
            SynchronizationContext.Current.Post(o =>
            {
                CheckDay(items);
            }, null);
            
        }

        public void PreviousMonth()
        {
            SetMonth(GetMonth().AddMonths(-1));
        }

        public void NextMonth()
        {
            SetMonth(GetMonth().AddMonths(1));
        }

        public async Task CheckTodayAsync()
        {
            if (IsChecked)
            {
                return;
            }
            var data = await _api.CheckInAsync();
            if (data == null || data.Data == null)
            {
                return;
            }
            SynchronizationContext.Current.Post(o =>
            {
                IsChecked = true;
                Tip = App.GetString("check_in_tip").Replace("{day}", data.Data.Running.ToString());
                var now = DateTime.Now.Year + "-" + DateTime.Now.Month.ToString("00");
                if (now == Month)
                {
                    CheckDay(DateTime.Now.Day);
                }
            }, null);
        }

        public void CheckDay(params int[] days)
        {
            if (days.Length < 1)
            {
                return;
            }
            foreach (var item in Days)
            {
                if (item.Day > 0 && days.Contains(item.Day))
                {
                    item.IsChecked = true;
                }
            }
            

        }

    }
}
