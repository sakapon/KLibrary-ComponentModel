using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BmiWpf
{
    [ValueConversion(typeof(double), typeof(double))]
    public class ScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var scale = GetScale(parameter);
            return
                (value is double) ? scale * (double)value :
                (value is int) ? scale * (int)value :
                DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var scale = GetScale(parameter);
            return
                (value is double) ? (double)value / scale :
                (value is int) ? (int)value / scale :
                DependencyProperty.UnsetValue;
        }

        static readonly Func<object, double> GetScale = o =>
        {
            try
            {
                return System.Convert.ToDouble(o);
            }
            catch (Exception)
            {
                return 1.0;
            }
        };
    }
}
