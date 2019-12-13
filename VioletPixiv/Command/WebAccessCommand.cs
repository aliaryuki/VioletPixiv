using Microsoft.Win32;
using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace VioletPixiv.Command
{
    /// <summary>
    /// When Link Icon Click
    /// </summary>
    public class WebAccessCommand : CommadBase
    {
        // [override] CommadBase
        public override void Execute(object parameter)
        {

            var TargetUrl = parameter as string;

            // Open In Browser
            System.Diagnostics.Process.Start(TargetUrl);

        }

    }
}
