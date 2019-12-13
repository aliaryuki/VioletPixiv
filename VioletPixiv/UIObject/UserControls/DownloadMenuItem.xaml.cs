using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class DownloadMenuItem : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
        #region DependencyProperty

        public static readonly DependencyProperty TargetImageProperty =
            DependencyProperty.Register("TargetImage", typeof(BitmapImage), typeof(DownloadMenuItem), new PropertyMetadata(TargetImageChanged));
        private BitmapImage _TargetImage = null;
        public BitmapImage TargetImage
        {
            get
            {
                return _TargetImage;
            }
            set
            {
                _TargetImage = value;
                OnPropertyChanged("TargetImage");
            }
        }

        private static void TargetImageChanged(Object d, DependencyPropertyChangedEventArgs e)
        {
            ((DownloadMenuItem)d).TargetImage = e.NewValue as BitmapImage;
        }

        #endregion

        public DownloadMenuItem()
        {
            InitializeComponent();

        }
    }
}
