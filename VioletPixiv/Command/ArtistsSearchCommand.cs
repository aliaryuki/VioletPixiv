using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace VioletPixiv.Command
{
    /// <summary>
    /// When "ArtistsSearch" Button Click in SearchingPage
    /// </summary>
    public class ArtistsSearchCommand : CommadBase
    {
        // [override] CommadBase
        public override void Execute(object parameter)
        {
            var values = parameter as object[];

            // Get SearchString and SortType
            var SearchString = values[0] as string;
            var SortType = values[1] as string;

            if (SearchString == null) return;

            MainWindow.PixivWindow.F3.NavigateJournal(new SearchArtistPage(SearchString, SortType));
        }
    }

}
