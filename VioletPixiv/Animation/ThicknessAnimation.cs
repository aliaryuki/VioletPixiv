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
    public enum ThicknessDirection
    {
        top,
        right,
        buttom,
        left,
        middle
    }

    public enum FadeMode
    {
        fadeIn,
        fadeOut
    }

    public static class LeftTabToggleAnimation
    {

        public static bool Toggle(Grid TargetGrid, double ToggleMarginCollapse, double ToggleMarginExpand, bool IsCollapse)
        {
            int TIMEDURATION = 400;
            var sb = new Storyboard();
            var MarginAnimation = new ThicknessAnimation();
            var fadeInEase = new CircleEase();

            if (IsCollapse)
            {
                // Expand Animation
                MarginAnimation = new ThicknessAnimation
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(TIMEDURATION)),
                    To = new Thickness(ToggleMarginExpand * -1, 0, ToggleMarginExpand, 0),
                    EasingFunction = fadeInEase
                };
            }
            else
            {
                // Collapse Animation
                MarginAnimation = new ThicknessAnimation
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(TIMEDURATION)),
                    To = new Thickness(ToggleMarginCollapse * -1, 0, ToggleMarginCollapse, 0),
                    EasingFunction = fadeInEase
                };
            }

            Storyboard.SetTargetProperty(MarginAnimation, new PropertyPath("Margin"));
            sb.Children.Add(MarginAnimation);

            sb.Begin(TargetGrid);
            return IsCollapse;
        }
    }

    public static class ThicknessInOutAnimation
    {
        public static void ThicknessFadeAnimation(FrameworkElement target, FadeMode mode, ThicknessDirection direction, int timeDuration, bool IsInnerGrid = false)
        {

            var sb = new Storyboard();
            var thick = new Thickness();
            var fadeInEase = new CircleEase();

            var fadeInAnimation = new ThicknessAnimation
            {

                Duration = new Duration(TimeSpan.FromMilliseconds(timeDuration)),
                //From = new Thickness(0, -SystemParameters.PrimaryScreenHeight, 0, SystemParameters.PrimaryScreenHeight),
                //To = null,
                EasingFunction = fadeInEase
            };

            var ThicknessHeight = (IsInnerGrid == false)? SystemParameters.PrimaryScreenHeight : 41;
            var ThicknessWidth = SystemParameters.PrimaryScreenWidth;

            switch (direction)
            {


                case ThicknessDirection.top:

                    thick = new Thickness(0, -ThicknessHeight, 0, ThicknessHeight);
                    break;

                case ThicknessDirection.right:

                    thick = new Thickness(ThicknessWidth, 0, -ThicknessWidth, 0);
                    break;

                case ThicknessDirection.buttom:

                    thick = new Thickness(0, ThicknessHeight, 0, -ThicknessHeight);
                    break;

                case ThicknessDirection.left:

                    thick = new Thickness(-ThicknessWidth, 0, ThicknessWidth, 0);
                    break;

                case ThicknessDirection.middle:

                    break;
            }

            switch (mode)
            {
                case FadeMode.fadeIn:
                    fadeInEase.EasingMode = EasingMode.EaseOut;
                    fadeInAnimation.From = thick;
                    break;

                case FadeMode.fadeOut:
                    fadeInEase.EasingMode = EasingMode.EaseIn;
                    fadeInAnimation.To = thick;
                    break;
            }

            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Margin"));
            sb.Children.Add(fadeInAnimation);

            sb.Begin(target);
        }
    }
}
