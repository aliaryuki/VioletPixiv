using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class MultiImagesOverlap
    {
        public MultiImagesOverlap()
        {
            InitializeComponent();

        }

        private void MultiImagesButton_Click(object sender, RoutedEventArgs e)
        {
            // Get All Images
            var a = (ObservableCollection<BitmapImage>)MultiImagesItems.ItemsSource;

            // Move Top To Buttom
            a.Move(a.Count() - 1, 0);
        }
    }
}
