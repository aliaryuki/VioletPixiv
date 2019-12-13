using Microsoft.Win32;
using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VioletPixiv.ViewModel;

namespace VioletPixiv
{
  
    public partial class PictureFrame
    {
        public PictureFrame(Illust targetIllust)
        {
            InitializeComponent();

            // Set ViewModel
            this.DataContext = new IllustDetailViewModel();

            // init
            FrameInit(targetIllust);
        }

        public void FrameInit(Illust TargetIllust)
        {
            // [No await] Update UserData and Illust Images
            _ = Task.Run(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
                {
                    await (this.DataContext as IllustDetailViewModel).GetTheIllust(TargetIllust);
                }));
            });
        }

     }
}
