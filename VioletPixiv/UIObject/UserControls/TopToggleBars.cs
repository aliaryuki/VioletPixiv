using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VioletPixiv.UIObject.UserControls
{
    public partial class TopToggleBar : ContentControl
    {
        public static readonly DependencyProperty HeadingProperty =DependencyProperty.Register("Heading", typeof(string), typeof(TopToggleBar), new PropertyMetadata(HeadingChanged));
        public string Heading { get; set; }

        private static void HeadingChanged(Object d, DependencyPropertyChangedEventArgs e)
        {
            ((TopToggleBar)d).Heading = e.NewValue as string;
        }
        
    }
}
