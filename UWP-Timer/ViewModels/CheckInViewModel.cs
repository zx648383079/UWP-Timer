﻿using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;
using UWP_Timer.Repositories;
using UWP_Timer.Utils;

namespace UWP_Timer.ViewModels
{
    public class CheckInViewModel: BindableBase
    {

        public CheckInViewModel()
        {
            _ = CanCheck();
            RefreshDay();
        }

        private string month;

        public string Month
        {
            get => month;
            set => Set(ref month, value);
        }

        private bool isChecked = false;

        public bool IsChecked
        {
            get => isChecked;
            set => Set(ref isChecked, value);
        }

        private string tip;

        public string Tip
        {
            get => tip;
            set => Set(ref tip, value);
        }


        private ObservableCollection<DayItem> days = new ObservableCollection<DayItem>();

        public ObservableCollection<DayItem> Days
        {
            get => days;
            set => Set(ref days, value);
        }

        public async Task CanCheck()
        {
            var data = await App.Repository.CheckIn.GetCanCheckInAsync();
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                if (data == null || data.Data == null)
                {
                    IsChecked = false;
                    Tip = String.Empty;
                    return;
                }
                IsChecked = true;
                Tip = Constants.GetString("check_in_tip").Replace("{day}", data.Data.Running.ToString());
            });
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
            var data = await App.Repository.CheckIn.GetMonthAsync(Month);
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
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                CheckDay(items);
            });
            
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
            var data = await App.Repository.CheckIn.CheckInAsync();
            if (data == null || data.Data == null)
            {
                return;
            }
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                IsChecked = true;
                Tip = Constants.GetString("check_in_tip").Replace("{day}", data.Data.Running.ToString());
                var now = DateTime.Now.Year + "-" + DateTime.Now.Month.ToString("00");
                if (now == Month)
                {
                    CheckDay(DateTime.Now.Day);
                }
            });
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
