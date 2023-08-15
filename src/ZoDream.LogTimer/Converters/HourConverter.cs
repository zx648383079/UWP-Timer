using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZoDream.LogTimer.Converters
{
    /// <summary>
    /// 时间格式化
    /// </summary>
    public class HourConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ConverterHelper.FormatHour((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
