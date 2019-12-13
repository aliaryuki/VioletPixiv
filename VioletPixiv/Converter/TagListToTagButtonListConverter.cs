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
using VioletPixiv.Command;

namespace VioletPixiv.Converter
{
    /// <summary>
    /// Conver TagList To TagButtonList
    /// </summary>
    public class TagListToTagButtonListConverter : IValueConverter
    {
        public object Convert(object value, Type targertType, object parameter, CultureInfo culture)
        {
            // Create List of Button
            List<Button> TagButtons = new List<Button>();

            foreach (var Tag in (List<Tag>)value)
            {
                // Add Style and Content
                Button tagButton = new Button()
                {
                    Style = Application.Current.Resources["LinkButtonStyle"] as Style,
                    Content = "#" + Tag.Name
                };

                // Add Command and CommandParameter
                tagButton.Command = Application.Current.FindResource("TagCommand") as TagCommand;
                tagButton.CommandParameter = Tag.Name;

                // Add To List
                TagButtons.Add(tagButton);
            }
            return TagButtons;
        }

        public object ConvertBack(object value, Type targertType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
