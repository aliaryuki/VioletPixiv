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
    /// When Artist Image Click in PictureFrame Tab2
    /// </summary>
    public class NewImageWindowCommand : CommadBase
    {
        // [override] CommadBase
        public override void Execute(object parameter)
        {

            var values = parameter as object[];
            var IllustTitle = values[0] as string;
            var IllustImage = values[1] as BitmapImage;

            // New BlurImage
            new VioletPixiv.UI.BlurImageWindow(IllustImage, IllustTitle).Show();

        }
    }

}
