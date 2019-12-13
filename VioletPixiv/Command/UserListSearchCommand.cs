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
    /// When "UserListsSearch" Button Click in SearchingPage
    /// </summary>
    public class UserListSearchCommand : CommadBase
    {
        // [override] CommadBase
        public override void Execute(object parameter)
        {
            var SearchString = parameter as string;
            if (SearchString == null) return;

            MainWindow.PixivWindow.F3.NavigateJournal(new UserDataFrame(Int32.Parse(SearchString)));
                
        }
    }

}
