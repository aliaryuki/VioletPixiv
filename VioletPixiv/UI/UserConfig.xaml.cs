using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VioletPixiv
{
    /// <summary>
    /// Interaction logic for UserConfig.xaml
    /// </summary>
    public partial class UserConfig : Page
    {
        public UserConfig()
        {
            InitializeComponent();
        }

        private void ThemeConfig_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary LightDic = new ResourceDictionary();
            LightDic.Source = new Uri("/MetroRadiance;component/Themes/Light.xaml", UriKind.RelativeOrAbsolute);

            ResourceDictionary DarkDic = new ResourceDictionary();
            DarkDic.Source = new Uri("/MetroRadiance;component/Themes/Dark.xaml", UriKind.RelativeOrAbsolute);

            if ((sender as ToggleButton).IsChecked ?? false || (sender as ToggleButton).IsChecked == false)
            {
                Properties.Settings.Default.DarkTheme = true;
                Properties.Settings.Default.Save();

                Application.Current.Resources.MergedDictionaries.Remove(LightDic);
                Application.Current.Resources.MergedDictionaries.Add(DarkDic);
            }
            else
            {
                Properties.Settings.Default.DarkTheme = false;
                Properties.Settings.Default.Save();

                Application.Current.Resources.MergedDictionaries.Remove(DarkDic);
                Application.Current.Resources.MergedDictionaries.Add(LightDic);
            }
        }

        private void NSFWConfig_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as ToggleButton).IsChecked ?? false || (sender as ToggleButton).IsChecked == false)
            {
                Properties.Settings.Default.NSFW = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.NSFW = false;
                Properties.Settings.Default.Save();
            }
        }
    }
}
