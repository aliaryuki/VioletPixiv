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
    /// When Tag String Click
    /// </summary>
    public class TagCommand : CommadBase
    {
        /// <summary>
        /// Cloase all SubPageFrames and set SelectedIndex = 3 (SearchArtistPage.Xaml)
        /// </summary>
        /// [override] CommadBase
        public override void Execute(object parameter)
        {
            // Get the Searching String
            var TargetSearchString = parameter as String;

            // Get All SubPageFrames
            var SubPageFrameList = UIAccess.FindChildrenByType<SubPageFrame>(MainWindow.PixivWindow.MainGridR1);

            foreach(var i in SubPageFrameList)
            {
                // [No await] Close all Frames
                _ = i.FrameOut();
            }

            // Search and Change to SearchPage 
            MainWindow.PixivWindow.F3.NavigateJournal(new SearchArtistPage(TargetSearchString, "新しい順"));
            MainWindow.PixivWindow.LeftTab.SelectedIndex = 3;
        }
    }

}
