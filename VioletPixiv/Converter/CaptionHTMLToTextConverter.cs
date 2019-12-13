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
    public class CaptionHTMLToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targertType, object parameter, CultureInfo culture)
        {
            return Parser(value as String);
        }

        public object ConvertBack(object value, Type targertType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private String Parser(string TargetString)
        {
            // Replace HTML Label Element
            TargetString = Regex.Replace(TargetString, @"<br />", "\n");
            TargetString = Regex.Replace(TargetString, "<[^>]*>", String.Empty);

            return TargetString;
        }
    }
}
