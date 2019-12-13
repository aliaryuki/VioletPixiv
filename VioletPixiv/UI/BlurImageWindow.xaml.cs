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

namespace VioletPixiv.UI
{
    /// <summary>
    /// Interaction logic for BlurImageWindow.xaml
    /// </summary>
    public partial class BlurImageWindow
    {
        public BlurImageWindow(BitmapImage imageSource,String IllustTitle)
        {
            InitializeComponent();
            MainImage.Source = imageSource;
            this.Title = IllustTitle;
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
