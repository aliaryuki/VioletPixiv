using Microsoft.Win32;
using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using VioletPixiv.UIObject.UserControls;

namespace VioletPixiv.Command
{
    /// <summary>
    /// When BookMarkButton Click
    /// </summary>
    public class BookMarkCommand : CommadBase
    {
        // [override] CommadBase
        public override void Execute(object parameter)
        {
            // Get Button and Illust
            var TargetBookMarkButton = parameter as BookMarkButton;
            var TargetIllust = TargetBookMarkButton.TargetIllust as Illust;

            // Set To BookMarked
            if (TargetBookMarkButton.IsChecked ?? false || TargetBookMarkButton.IsChecked == false) 
            {
                // [No await] API - Add Illust To Bookmark
                _ = MainWindow.PixivWindow.AuthToken.AddIllustToBookmark(TargetIllust.Id);
            }
            // Set to UnBookMarked
            else 
            {
                // [No await] API - Delete Illust From Bookmarked
                _ = MainWindow.PixivWindow.AuthToken.DeleteIllustFromBookmarked(TargetIllust.Id);
            }
        }
    }
}
