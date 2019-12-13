using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using static VioletPixiv.UIObject.EnumType;

namespace VioletPixiv
{
    public partial class SearchArtistPage : IllustsPageTemplate
    {
        /// <summary>
        /// Mapping Sting To IllustsSearchSortType
        /// </summary>
        public Dictionary<String, IllustsSearchSortType> TypeChange = new Dictionary<String, IllustsSearchSortType>()
        {
            { "新しい順", IllustsSearchSortType.date_desc },
            { "古い順", IllustsSearchSortType.date_asc },
            { "人気い順", IllustsSearchSortType.popular_desc }
        };

        public SearchArtistPage(string searchKeyWord, string SortType = null)
        {
            InitializeComponent();

            // Set Title
            this.Title = searchKeyWord + " (" + SortType + ")";

            // Set ViewModel
            this.DataContext = new IllustsSearchViewModel(searchKeyWord, TypeChange[SortType] );

            // init
            this.InitLoading();
            
        }

    }
}
