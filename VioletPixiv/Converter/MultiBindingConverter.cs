using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace VioletPixiv.Converter
{
    /// <summary>
    /// Conver MultiBinding Values To Command Used
    /// </summary>
    public class MultiBindingConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Conver To Margin Value
    /// </summary>
    public class ThicknessBindingConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var a = (double)values[0];
            var b = (double)values[1];
            var c = (double)values[2];
            var d = c * 0.75 + a -b;

            return new Thickness(d * -1, 0, d, 0);
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
