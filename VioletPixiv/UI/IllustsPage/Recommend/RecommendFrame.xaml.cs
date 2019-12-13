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
using static VioletPixiv.UIObject.EnumType;

namespace VioletPixiv
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class RecommendFrame
    {
        public RecommendFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Navigate
        /// </summary>
        private void MenuCombobox_Changed(object sender, SelectionChangedEventArgs e)
        {
            var TargetElement = (ComboBox)sender;

            switch ((TargetElement.SelectedItem as ComboBoxItem).Content as string)
            {
                case "イラスト":
                    this.Navigate(new RecommendPage(IllustsRecommendedType.illust));
                    break;
            
                case "マンガ":
                    this.Navigate(new RecommendPage(IllustsRecommendedType.manga));
                    break;
            }

        }
    }
}
