using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace UWP_Timer.Converters
{
    /// <summary>
    /// 圆进度转化
    /// </summary>
    public class ProgressConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // 边的粗细
            double thine = 3;
            // 300 为圆的外直径
            double w = 300 - thine;
            double n = Math.PI * w / thine * (double)value / 100;
            return new DoubleCollection() {
               n,
                1000
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
