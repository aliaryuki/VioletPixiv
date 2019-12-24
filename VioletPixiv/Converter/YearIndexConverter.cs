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
    public class YearIndexConverter : IValueConverter
    {
        DateTime TodayDate = DateTime.Today.ToLocalTime();

        public object Convert(object value, Type targertType, object parameter, CultureInfo culture)
        {
            var YearIndex = (int)value;
            return TodayDate.Year - YearIndex;
        }

        public object ConvertBack(object value, Type targertType, object parameter, CultureInfo culture)
        {
            var YearIndex = (int)value;
            return TodayDate.Year - YearIndex;
        }
    }
}
