using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace VioletPixiv.Converter
{
    /// <summary>
    /// Convert HTML To Plain Text
    /// </summary>
    public class MonthIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targertType, object parameter, CultureInfo culture)
        {
            var MonthIndex = (int)value;
            return MonthIndex - 1;
        }

        public object ConvertBack(object value, Type targertType, object parameter, CultureInfo culture)
        {
            var MonthIndex = (int)value;
            return MonthIndex + 1;
        }
    }
}
