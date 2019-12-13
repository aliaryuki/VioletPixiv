using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace VioletPixiv
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class SearchArtistFrame
    {
        public SearchArtistFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Change to history search
        /// </summary>
        private void HistoryCombobox_Changed(object sender, SelectionChangedEventArgs e)
        {
            var TargetElement = sender as ComboBox;
            if ((this.CanGoBack || this.CanGoForward) && TargetElement.IsDropDownOpen)
            {
                int ForwardStackLength = this.ForwardStack.OfType<JournalEntry>().Count();

                // Count how many times should GoBack(GoForward)
                var Counter = TargetElement.SelectedIndex - ForwardStackLength;
                if (Counter > 0)
                    while (Counter-- > 0)
                    {
                        this.GoBack();
                    }
                else
                    while (Counter++ < 0)
                    {
                        this.GoForward();
                    }
            }
            else
            {
                TargetElement.SelectedIndex = (this.CanGoForward) ? this.ForwardStack.OfType<JournalEntry>().Count() : 0;
            }
        }

    }
}
