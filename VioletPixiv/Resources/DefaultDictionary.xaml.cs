using Microsoft.Win32;
using Pixeez.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using VioletPixiv.Animation;

namespace VioletPixiv
{
    public partial class DefaultDictionary
    {
        private void MenuToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var TargetElement = (ToggleButton)sender;
            var MainDockPanel = UIAccess.FindParentByType<DockPanel>(TargetElement);

            Grid ToggleGrid = MainDockPanel.FindName("ToggleGrid") as Grid;

            if ((sender as ToggleButton).IsChecked ?? false || (sender as ToggleButton).IsChecked == false)
            {
                Task.Run(async () =>
                {
                    await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
                    {
                        ToggleGrid.Visibility = Visibility.Visible;
                        await FrameIn(ToggleGrid);
                    }));
                });
                 
            }
            else
            {
                Task.Run(async () =>
                {
                    await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
                    {
                        await FrameOut(ToggleGrid);
                        ToggleGrid.Visibility = Visibility.Collapsed;
                    }));
                });
                
            }
        }

        private async Task FrameIn(FrameworkElement TargetElement)
        {

            var FADETIME = 300;
            OpacityInOutAnimation.OpacityFadeAnimation(TargetElement, null, 1, FADETIME);
            ThicknessInOutAnimation.ThicknessFadeAnimation(TargetElement, FadeMode.fadeIn, ThicknessDirection.top, FADETIME, IsInnerGrid : true);
            await Task.Delay(FADETIME);
        }

        private async Task FrameOut(FrameworkElement TargetElement)
        {
            var FADETIME = 300;
            OpacityInOutAnimation.OpacityFadeAnimation(TargetElement, null, 0, FADETIME);
            ThicknessInOutAnimation.ThicknessFadeAnimation(TargetElement, FadeMode.fadeOut, ThicknessDirection.top, FADETIME, IsInnerGrid: true);
            await Task.Delay(FADETIME);
        }

    }
}
