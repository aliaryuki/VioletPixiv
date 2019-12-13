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
    /// When Download Button Click in Right-Click menu from Illust Image
    /// </summary>
    public class DownloadCommand : CommadBase
    {
        // [override] CommadBase
        public override void Execute(object parameter)
        {

            var TargetImage = parameter as BitmapImage;

            SaveFileDialog f = new SaveFileDialog
            {
                FileName = RandomHash(),
                Filter = "*.jpg|*.png"
            };

            if (f.ShowDialog() == true)
            {
                Task.Run(
                    () => Save(TargetImage, f.FileName)
                );
            }

        }

        private void Save(BitmapImage image, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        private string RandomHash()
        {
            var bytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }

            return BitConverter.ToString(bytes);
        }

    }

}
