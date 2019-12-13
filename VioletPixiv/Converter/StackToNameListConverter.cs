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
    /// Integrate multiple Stack into a List
    /// </summary>
    public class StackToNameListConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var TargetBackStack = (System.Collections.IEnumerable)values[0];
            var TargetForwardStack = (System.Collections.IEnumerable)values[1];
            var TargetTag = (System.Windows.Controls.Page)values[2];

            if (TargetTag == null) return 0;

            List<string> MenuList = new List<string>();

            // ADD ForwardStack
            if(TargetForwardStack != null)
            foreach (JournalEntry i in TargetForwardStack)
            {
                MenuList.Add(i.Name);
            }
            MenuList.Reverse();

            // ADD Myself
            MenuList.Add(TargetTag.Title);

            // ADD BackStack
            if (TargetBackStack != null)
            foreach (JournalEntry i in TargetBackStack)
            {
                MenuList.Add(i.Name);
            }

            return MenuList;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
