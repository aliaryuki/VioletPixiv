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
    public partial class BookMarkButton
    {
        #region new (Illust) TargetIllust DependencyProperty

        public static readonly DependencyProperty TargetIllustProperty = 
            DependencyProperty.Register("TargetIllust", typeof(Illust), typeof(BookMarkButton), new PropertyMetadata(TargetIllustChanged));
        public Illust TargetIllust { get; set; }

        private static void TargetIllustChanged(Object d, DependencyPropertyChangedEventArgs e)
        {
            ((BookMarkButton)d).TargetIllust = e.NewValue as Illust;
        }

        #endregion

        public BookMarkButton()
        {
            InitializeComponent();
        }
    }
}
