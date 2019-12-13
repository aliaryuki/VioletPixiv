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
    /// When HeaderImage Click
    /// </summary>
    public class HeaderCommand : CommadBase
    {
        // [override] CommadBase
        public override void Execute(object parameter)
        {
            // Get UserId
            var TargetUserId = (long)parameter;

            // New SubPageFrame
            new SubPageFrame(MainWindow.PixivWindow.MainGridR1, new UserDataFrame(TargetUserId));
        }
    }


}
