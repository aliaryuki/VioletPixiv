using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace VioletPixiv.Converter
{
    /// <summary>
    /// Count IEnumerable Length
    /// </summary>
    public class StackCountConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var TargetForwardStack = (System.Collections.IEnumerable)values[0];
            int ForwardStackLength = (TargetForwardStack == null) ? 0 : TargetForwardStack.OfType<JournalEntry>().Count();
            return ForwardStackLength;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
