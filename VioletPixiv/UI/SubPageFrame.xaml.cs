using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VioletPixiv.Animation;

namespace VioletPixiv
{
    public partial class SubPageFrame : UserControl
    {
        // Animation Duration
        static readonly int FADEDURATOIN = 300;

        public SubPageFrame(Panel TargetPanel, Page TargetPage)
        {
            InitializeComponent();

            // Put Page into Frame
            MainFrame.Navigate(TargetPage);
            
            // Start Animation
            FrameIn();

            // Put Frame into Panel
            TargetPanel.Children.Add(this);

        }

        /// <summary>
        /// FadeIn From Top
        /// </summary>
        public Task FrameIn()
        {
            OpacityInOutAnimation.OpacityFadeAnimation(this, 0, null, FADEDURATOIN);
            ThicknessInOutAnimation.ThicknessFadeAnimation(this, FadeMode.fadeIn, ThicknessDirection.top, FADEDURATOIN);

            return Task.CompletedTask;
        }

        /// <summary>
        /// FadeOut To Top
        /// </summary>
        public Task FrameOut()
        {
            OpacityInOutAnimation.OpacityFadeAnimation(this, null, 0, FADEDURATOIN);
            ThicknessInOutAnimation.ThicknessFadeAnimation(this, FadeMode.fadeOut, ThicknessDirection.top, FADEDURATOIN);

            return Task.CompletedTask;
        }

    }
}
