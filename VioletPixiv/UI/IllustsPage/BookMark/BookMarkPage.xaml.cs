using Pixeez.Objects;
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
using VioletPixiv.ViewModel;

namespace VioletPixiv
{

    public partial class BookMarkPage : IllustsPageTemplate
    {
        /// <summary>
        /// Show BookMarkPage
        /// </summary>
        /// <param name="illustID"> Illust ID </param>
        public BookMarkPage(long illustID)
        {
            InitializeComponent();

            // Set ViewModel
            this.DataContext = new IllustsBookmarksViewModel(illustID);

            // init
            this.InitLoading();

        }

    }
}
