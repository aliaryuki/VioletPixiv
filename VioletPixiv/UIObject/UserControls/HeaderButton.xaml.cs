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
    public partial class HeaderButton
    {
        #region TargetUserID Property

        public static readonly DependencyProperty TargetUserIDProperty = DependencyProperty.Register("TargetUserID", typeof(long), typeof(HeaderButton), new PropertyMetadata(TargetUserIDChanged));
        public long TargetUserID { get; set; }

        private static void TargetUserIDChanged(Object d, DependencyPropertyChangedEventArgs e)
        {
            ((HeaderButton)d).TargetUserID = (long)e.NewValue;
        }

        #endregion

        public HeaderButton()
        {
            InitializeComponent();
        }
    }
}
