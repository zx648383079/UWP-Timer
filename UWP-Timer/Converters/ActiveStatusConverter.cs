﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UWP_Timer.Converters
{
    /// <summary>
    /// 活动正在进行显示
    /// </summary>
    public class ActiveStatusConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ConverterHelper.VisibleIf((int)value == 9);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
