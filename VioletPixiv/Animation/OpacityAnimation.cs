using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace VioletPixiv.Animation
{
    public static class OpacityInOutAnimation
    {
        public static void OpacityFadeAnimation(FrameworkElement target, double? opacityFrom, double? opacityTo, int timeDuration)
        {
            var sb = new Storyboard();
            var fadeInAnimation = new DoubleAnimation
            {

                Duration = new Duration(TimeSpan.FromMilliseconds(timeDuration)),
                From = opacityFrom,
                To = opacityTo
            };

            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity"));
            sb.Children.Add(fadeInAnimation);

            sb.Begin(target);
        }

    }
}
