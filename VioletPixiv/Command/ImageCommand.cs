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
    /// When Illust Image Click
    /// </summary>
    public class ImageCommand : CommadBase
    {
        // [override] CommadBase
        public override void Execute(object parameter)
        {
            // Get Illust
            var TargetIllust = parameter as Illust;

            // New SubPageFrame
            new SubPageFrame(MainWindow.PixivWindow.MainGridR1, new PictureFrame(TargetIllust));
        }
    }

}
