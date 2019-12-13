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
    /// When X Button Click
    /// </summary>
    public class CloseFrameCommand : CommadBase
    {
        // [override] CommadBase
        public override void Execute(object parameter)
        {
            try
            {
                // Get the Frame
                var TargergetFrame = UIAccess.FindParentByType<SubPageFrame>(parameter as Button);

                // [No await] Close the Frame
                _ = TargergetFrame.FrameOut();
            }
            catch { }
        }
    }

}
