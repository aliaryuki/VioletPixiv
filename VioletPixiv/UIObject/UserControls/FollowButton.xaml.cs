using Pixeez.Objects;
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

namespace VioletPixiv.UIObject.UserControls
{

    public partial class FollowButton
    {

        #region new (Illust) TargetIllust DependencyProperty

        public static readonly DependencyProperty TargetUserProperty =
            DependencyProperty.Register("TargetUser", typeof(User), typeof(FollowButton), new PropertyMetadata(TargetUserChanged));
        public User TargetUser { get; set; }

        private static void TargetUserChanged(Object d, DependencyPropertyChangedEventArgs e)
        {
            ((FollowButton)d).TargetUser = e.NewValue as User;
        }

        #endregion


        public FollowButton()
        {
            InitializeComponent();
        }
    }
}
